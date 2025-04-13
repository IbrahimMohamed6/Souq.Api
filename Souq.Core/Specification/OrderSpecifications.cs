using Souq.Core.Entites.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Specification
{
   public class OrderSpecifications:BaseSpecification<Order>
    {
        public OrderSpecifications(string Email)
            :base(o=>o.BuyerEmail==Email)
        {
            Include.Add(o => o.DeliveryMethod);
            Include.Add(o => o.Items);
            AddOrderbyDesc(o => o.OrderDate);
        }

        public OrderSpecifications(string Email,int Id)
            : base(o => o.BuyerEmail == Email&&o.Id==Id)
        {
            Include.Add(o => o.DeliveryMethod);
            Include.Add(o => o.Items);
            AddOrderbyDesc(o => o.OrderDate);
        }
    }
}
