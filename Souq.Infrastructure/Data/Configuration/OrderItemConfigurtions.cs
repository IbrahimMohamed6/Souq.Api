using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Souq.Core.Entites.OrderAggregate;


namespace Souq.Infrastructure.Data.Configuration
{
    public class OrderItemConfigurtions : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(o => o.Product, o => o.WithOwner());

            builder.Property(o => o.Price).
               HasColumnType("decimal(18,2)");


        }
    }
}
