using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiDuongScada.Domain.AggregateModels.MouldAggregate;

namespace ThaiDuongScada.Infrastructure.EntityConfigurations;
public class MouldEntityTypeConfiguration : IEntityTypeConfiguration<Mould>
{
    public void Configure(EntityTypeBuilder<Mould> builder)
    {
        builder.HasKey(x => x.MouldId);
    }
}
