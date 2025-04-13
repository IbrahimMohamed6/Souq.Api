using Souq.Core.Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Specification
{
   public class ProductByCategoryNameSpec:BaseSpecification<Product>
    {
        public ProductByCategoryNameSpec(string categoryName):base(p=>p.Category.Name==categoryName)
        {
            
        }
    }
}
