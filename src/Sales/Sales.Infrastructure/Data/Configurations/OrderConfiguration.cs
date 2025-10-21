using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sales.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).HasConversion(
                orderId => orderId.Value,
                dbId => OrderId.Of(dbId));

            builder.HasOne<QuoteRequest>()
                   .WithMany()
                   .HasForeignKey(o => o.QuoteRequestId)
                   .IsRequired();

            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Processing)
                .HasConversion(
                    status => status.ToString(),
                    dbStatus => Enum.Parse<OrderStatus>(dbStatus));

        }
    }
}
