using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Souq.Core.Entites.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Infrastructure.Data.Configuration
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status)
                .HasConversion(s => s.ToString(), s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s));
            builder.OwnsOne(o => o.ShippingAddress, o => o.WithOwner());

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .HasForeignKey(o => o.DeliveryMethodId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Property(o => o.SubTotal).
                        HasColumnType("decimal(18,2)");
        }
    }
}
