using Souq.Core.Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.RepositoryContract
{
    public interface IProductRepository
    {
        public Task<IReadOnlyList<Product>> GetAllProductByBrand(string brandName);
        public Task<IReadOnlyList<Product>> GetAllProductByCategory(string categoryName);
        // Add Product
        public Task<Product> AddProduct(Product product);
        // Update Product
        public Task<Product> UpdateProduct(Product product);

    }
}
