using Souq.Core.Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }

        public List<Expression<Func<T,object>>> Include { get ; set ; }= new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> Orderby { get ; set; }

        public Expression<Func<T, object>> OrderbyDesc { get ; set ; }
        public int Skip { get ; set; }
        public int Take { get ; set; }
        public bool IsPaginated { get; set; }

        public BaseSpecification()
        {
            
        }

        public BaseSpecification(Expression<Func<T, bool>> _Criteria)
        {
            Criteria = _Criteria;
            Include = new List<Expression<Func<T, object>>>();
        }

        public void AddOrderby(Expression<Func<T, object>>  OrderbyExpression)
        {
            Orderby = OrderbyExpression;
        }

        public void AddOrderbyDesc(Expression<Func<T, object>> OrderbyDescExpression)
        {
            OrderbyDesc = OrderbyDescExpression;
        }

        public void ApplyPaging(int skip, int take)
        {
            IsPaginated = true;
            Skip = skip;
            Take = take;
        }
    }
}
