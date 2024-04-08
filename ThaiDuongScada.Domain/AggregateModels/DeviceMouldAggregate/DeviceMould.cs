using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
using ThaiDuongScada.Domain.AggregateModels.MouldAggregate;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;

namespace ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;
public class DeviceMould: IAggregateRoot
{
    public string DeviceId { get; set; }
    public int MouldSlot { get; set; }
    public int PreviousShotCount { get; set; }
    public Device Device { get; set; }
    public Mould Mould { get; set; }
    public EShiftDuration ShiftDuration { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private DeviceMould() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DeviceMould(int mouldSlot, int previousShotCount, string deviceId, Device device, Mould mould, EShiftDuration shiftTime)
    {
        MouldSlot = mouldSlot;
        PreviousShotCount = previousShotCount;
        DeviceId = deviceId;
        Device = device;
        Mould = mould;
        ShiftDuration = shiftTime;
    }

    public void SetPreviousShotCount(int previousShotCount)
    {
        PreviousShotCount = previousShotCount;
    }
}
