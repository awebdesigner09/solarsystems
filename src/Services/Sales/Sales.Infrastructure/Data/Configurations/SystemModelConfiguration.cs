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
                .IsRequired();

            builder.Property(sm => sm.BasePrice)
                .IsRequired();
        }
    }
}
