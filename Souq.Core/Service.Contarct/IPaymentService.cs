using Souq.Core.Entites.Basket;
using Souq.Core.Entites.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Service.Contarct
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOreUpdatePaymentIntent(string basketId);
        Task<Order> UpdateOrderStatus(string paymentIntentId, bool flag);


    }
}
