using Souq.Core.Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Specification
{
    public interface ISpecification< T> where T : BaseEntity
    {
        public Expression<Func<T,bool>> Criteria { get; set; }

        public List<Expression<Func<T, object>>> Include { get; set; }

        public Expression<Func<T,object>> Orderby { get; set; }

        public Expression<Func<T,object>> OrderbyDesc { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginated { get; set; }
    }
}
