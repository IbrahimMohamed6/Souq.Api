using Souq.Core.Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Specification
{
   public class ProductByBrandNameSpec:BaseSpecification<Product>
    {
        public ProductByBrandNameSpec(string brandName):base(p=>p.Brand.Name==brandName)
        {
            
        }
    }
}
