using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Souq.Core.Entites.Basket;
using Souq.Core.Entites.OrderAggregate;
using Souq.Core.RepositoryContract;
using Souq.Core.Service.Contarct;
using Souq.Core.Specification;
using Stripe;
using Product = Souq.Core.Entites.Products.Product;


namespace Souq.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketReposatory _basketReposatory;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration
            , IBasketReposatory basketReposatory
            , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketReposatory = basketReposatory;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOreUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StrpeSetting:Secretkey"];
            var Basket = await _basketReposatory.GetBasket(basketId);
            if (Basket is null)
                return null;
            var ShippingAddress = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMehod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingAddress = DeliveryMehod.Cost;
            }

            if (Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }
            var SubTotal = Basket.Items.Sum(s => s.Price * s.Quantity);

            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingAddress * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await Service.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingAddress * 100,
                };
                paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;

            }
            await _basketReposatory.UpdateBasket(Basket);
            return Basket;
        }






        public async Task<Order> UpdateOrderStatus(string paymentIntentId, bool flag)
        {
            var spec = new OrderWithPaymentSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(order);
              await _unitOfWork.SaveChangesAsyc();
            return order;
        }
    }
}
