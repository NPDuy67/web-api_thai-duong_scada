using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;

namespace ThaiDuongScada.Infrastructure.EntityConfigurations;
public class ShiftReportEntityTypeConfiguration : IEntityTypeConfiguration<ShiftReport>
{
    public void Configure(EntityTypeBuilder<ShiftReport> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasOne(p => p.Device)
            .WithMany()
            .HasForeignKey(p => p.DeviceId);

        builder.OwnsMany(p => p.Shots, p =>
        {
            p.WithOwner();
            p.Property(p => p.TimeStamp).IsRequired();
            p.Property(p => p.InjectionTime).IsRequired();
            p.Property(p => p.InjectionCycle).IsRequired();
        });
    }
}
