using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;

namespace ThaiDuongScada.Domain.AggregateModels.MachineStatusAggregate;

public class MachineStatus : IAggregateRoot
{
    public int Id { get; set; }
    public string DeviceId { get; set; }
    public Device Device { get; set; }
    public int MouldSlot { get; set; }
    public EShiftDuration ShiftDuration { get; set; }
    public int ShiftNumber { get; set; }
    public DateTime Date { get; set; } 
    public EMachineStatus Status { get; set; }
    public DateTime Timestamp { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private MachineStatus() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public MachineStatus(int id, string deviceId, Device device, int mouldSlot, EShiftDuration shiftDuration, int shiftNumber ,DateTime date, EMachineStatus status, DateTime timestamp)
    {
        Id = id;
        DeviceId = deviceId;
        Device = device;
        MouldSlot = mouldSlot;
        ShiftDuration = shiftDuration;
        ShiftNumber = shiftNumber;
        Date = date;
        Status = status;
        Timestamp = timestamp;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public MachineStatus(int mouldSlot, Device device, EShiftDuration shiftDuration, EMachineStatus status, DateTime timestamp)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        var shiftNumber = ShiftTimeHelper.GetShiftNumber(timestamp, shiftDuration);
        var date = ShiftTimeHelper.GetShiftDate(timestamp, shiftDuration);
        MouldSlot = mouldSlot;
        Device = device;
        ShiftDuration = shiftDuration;
        ShiftNumber = shiftNumber;
        Date = date;
        Status = status;
        Timestamp = timestamp;
    }
}
