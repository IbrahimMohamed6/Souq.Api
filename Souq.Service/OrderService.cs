using Souq.Core.Entites.OrderAggregate;
using Souq.Core.Entites.Products;
using Souq.Core.RepositoryContract;
using Souq.Core.Service.Contarct;
using Souq.Core.Specification;

namespace Souq.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketReposatory _basketReposatory;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketReposatory basketReposatory
            ,IPaymentService paymentService
            ,IUnitOfWork unitOfWork)
        {
            _basketReposatory = basketReposatory;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Order> CrateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, OrderAddress ShippingAddress)
        {

            //1.Get Basket From Basket Repo
            var Basket = await _basketReposatory.GetBasket(BasketId);
            //2.Get Selected Items at Basket From Product Repo

            var Items = new List<OrderItem>();
            foreach (var item in Basket.Items)
            {
               if(Basket?.Items.Count>0)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItem = new ProductItemOrder(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItem, Product.Price, item.Quantity);
                    Items.Add(OrderItem);
                }
            }
            //3.Calculate SubTotal
            var subTotal =Items.Sum(i => i.Price * i.Quantity);
            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            //5.Create Order
            var Spec = new OrderWithPaymentSpecification(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOreUpdatePaymentIntent(BasketId);

            }
            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, Items, subTotal,Basket.PaymentIntentId);
            //6.Add Order Locally
           await _unitOfWork.Repository<Order>().AddAsync(Order);
            //7.Save Order To Database [ToDo]
          var Result=  await _unitOfWork.SaveChangesAsyc();
            if (Result <= 0)
                return null;
            return Order;

        }

        public Task<Order> GetByIdOrderForSpecificUser(string BuyerEmail, int OrderId)
        {
            var Spec = new OrderSpecifications(BuyerEmail, OrderId);
            var Orders = _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
             return Orders;

        }

        public Task<IReadOnlyList<Order>> GetOrderForSpecificUser(string BuyerEmail)
        {
            var Spec = new OrderSpecifications(BuyerEmail);
            var Orders = _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return Orders;
        }

        public Task<Order> UpdateOrderStatus(string paymentIntentId, bool flag)
        {
            return _paymentService.UpdateOrderStatus(paymentIntentId, flag);


        }
    }
}
