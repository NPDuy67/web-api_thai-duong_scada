using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;

namespace ThaiDuongScada.Infrastructure.EntityConfigurations;
public class DeviceMouldEntityTypeConfiguration : IEntityTypeConfiguration<DeviceMould>
{
    public void Configure(EntityTypeBuilder<DeviceMould> builder)
    {
        builder.HasKey(x => new { x.DeviceId, x.MouldSlot });

        builder.HasOne(p => p.Device)
            .WithMany()
            .HasForeignKey(p => p.DeviceId)
            .IsRequired();
        builder.HasOne(p => p.Mould)
            .WithMany()
            .IsRequired();
    }
}
