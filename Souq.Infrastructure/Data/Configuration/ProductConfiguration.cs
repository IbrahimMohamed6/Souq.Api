
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Souq.Core.Entites.Products;

namespace Souq.Infrastructure.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.Brand).WithMany()
                .HasForeignKey(p => p.BrandId);

            builder.HasOne(P => P.Category).WithMany()
                .HasForeignKey(P => P.CategoryId);
        }
    }
}
