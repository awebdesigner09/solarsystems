using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sales.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                customerId => customerId.Value,
                dbId => CustomerId.Of(dbId));
            
            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.ComplexProperty(
                
                c => c.Address,
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
        }
    }
}
