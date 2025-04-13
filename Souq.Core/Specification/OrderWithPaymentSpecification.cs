using Souq.Core.Entites.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Specification
{
   public class OrderWithPaymentSpecification :BaseSpecification<Order>
    {
        public OrderWithPaymentSpecification(string PaymentIntentId)
            : base(o => o.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
