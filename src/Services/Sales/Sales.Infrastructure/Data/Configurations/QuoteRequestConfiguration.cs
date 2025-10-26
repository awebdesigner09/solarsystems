using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sales.Infrastructure.Data.Configurations
{
    public class QuoteRequestConfiguration : IEntityTypeConfiguration<QuoteRequest>
    {
        public void Configure(EntityTypeBuilder<QuoteRequest> builder)
        {
            builder.HasKey(qr => qr.Id);
            
            builder.Property(qr => qr.Id).HasConversion(
                quoteRequestId => quoteRequestId.Value,
                dbId => QuoteRequestId.Of(dbId));

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(qr => qr.CustomerId)
                .IsRequired();
            
            builder.HasOne<SystemModel>()
                .WithMany()
                .HasForeignKey(qr => qr.SystemModelId)
                .IsRequired();

            builder.Property(qr => qr.Status)
                .HasDefaultValue(QuoteRequestStatus.Pending)
                .HasConversion(
                    status => status.ToString(),
                    dbStatus => Enum.Parse<QuoteRequestStatus>(dbStatus));

            builder.Property(qr => qr.CustomConfig)
                .HasMaxLength(2000)
                .IsRequired(false);

        }
    }
}
