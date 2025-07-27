using Microsoft.EntityFrameworkCore;
using Souq.Core.Entites.Products;
using Souq.Core.RepositoryContract;
using Souq.Infrastructure.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

       

        public async Task<IReadOnlyList<Product>> GetAllProductByBrand(string brandName)
         => await _context.Set<Product>().Where(p => p.Brand.Name == brandName).ToListAsync();

        public async Task<IReadOnlyList<Product>> GetAllProductByCategory(string categoryName)
         => await _context.Set<Product>().Where(p => p.Category.Name == categoryName).ToListAsync();


        public Task<Product> AddProduct(Product product)
        {
            _context.Set<Product>().Add(product);
            return Task.FromResult(product);
        }
        public Task<Product> UpdateProduct(Product product)
        {
            _context.Set<Product>().Update(product);
            return Task.FromResult(product);

        }
    }
}
