using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sales.Infrastructure.Data.Configurations
{
    public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                .HasConversion(
                    quoteId => quoteId.Value,
                    dbId => QuoteId.Of(dbId));

            builder.HasOne<QuoteRequest>()
                .WithMany()
                .HasForeignKey(q => q.QuoteRequestId)
                .IsRequired();

            builder.Property(q => q.IssuedOn)
                .IsRequired();

            builder.Property(q => q.ValidUntil)
                .IsRequired();

            builder.OwnsOne(q => q.Components, componentsBuilder =>
            {
                componentsBuilder.Property(c => c.NoOfPanels).IsRequired();
                componentsBuilder.Property(c => c.NoOfInverters).IsRequired();
                componentsBuilder.Property(c => c.NoOfMoutingSystems).IsRequired();
                componentsBuilder.Property(c => c.NoOfBatteries).IsRequired();
            });

            builder.Property(q => q.BasePrice)
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(q => q.Tax1)
                .HasPrecision(18,2)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(q => q.Tax2)
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(q => q.TotalPrice)
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

        }
    }
}
