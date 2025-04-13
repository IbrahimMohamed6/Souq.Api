using Souq.Core.Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Specification
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams specParams)
            : base(x =>
                 (!specParams.BrandId.HasValue || x.BrandId == specParams.BrandId) &&
            (!specParams.CategoryId.HasValue || x.CategoryId == specParams.CategoryId)
                 )
        {

        }
    }
}
