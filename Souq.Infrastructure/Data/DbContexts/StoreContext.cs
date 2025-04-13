
using Microsoft.EntityFrameworkCore;
using Souq.Core.Entites.OrderAggregate;
using Souq.Core.Entites.Products;
using System.Reflection;

namespace Souq.Infrastructure.Data.DbContexts
{
  public  class StoreContext:DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }



        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryMethod> DeliveyMethods { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
