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
            
            builder.ComplexProperty(

                c => c.InstallationAddress,
                addressBuilder =>
                {
                    addressBuilder.Property(a => a.AddressLine1)
                        .HasMaxLength(200)
                        .IsRequired();

                    addressBuilder.Property(a => a.AddressLine2)
                        .HasMaxLength(200);

                    addressBuilder.Property(a => a.City)
                        .HasMaxLength(100)
                        .IsRequired();

                    addressBuilder.Property(a => a.State)
                        .HasMaxLength(100)
                        .IsRequired();

                    addressBuilder.Property(a => a.PostalCode)
                        .HasMaxLength(20)
                        .IsRequired();

                    addressBuilder.Property(a => a.Country)
                        .HasDefaultValue("USA");
                });

            builder.ComplexProperty(
                qr => qr.QuoteCustomOptions,
                optionsBuilder =>
                {
                    optionsBuilder.Property(o => o.OptBattery)
                    .HasDefaultValue(false);

                    optionsBuilder.Property(o => o.OptEVCharger)
                    .HasDefaultValue(false);
                });

            builder.Property(qr => qr.AdditonalNotes)
                .HasMaxLength(2000)
                .IsRequired(false);

        }
    }
}
