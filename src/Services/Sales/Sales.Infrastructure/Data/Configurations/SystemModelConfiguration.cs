using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sales.Infrastructure.Data.Configurations
{
    public class SystemModelConfiguration : IEntityTypeConfiguration<SystemModel>
    {
        public void Configure(EntityTypeBuilder<SystemModel> builder)
        {
            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.Id).HasConversion(
                systemModelId => systemModelId.Value,
                dbId => SystemModelId.Of(dbId));

            builder.Property(sm => sm.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(sm => sm.PanelType)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(sm => sm.CapacityKW)
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(sm => sm.BasePrice)
                .HasPrecision(18,2)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
