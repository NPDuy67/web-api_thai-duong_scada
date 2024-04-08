using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;

namespace ThaiDuongScada.Infrastructure.EntityConfigurations;
public class DeviceEntityTypeConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(p => p.DeviceId);
    }
}
