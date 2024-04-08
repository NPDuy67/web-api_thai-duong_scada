using ThaiDuongScada.Domain.AggregateModels.MachineStatusAggregate;

namespace ThaiDuongScada.Api.Application.Queries.MachineStatus;

public class MachineStatusViewModel
{
    public string DeviceId { get; set; }
    public int MouldSlot { get; set; }
    public EMachineStatus Status { get; set; }
    public int ShiftNumber { get; set; }
    public DateTime Date { get; set; }
    public DateTime Timestamp { get; set; }

    public MachineStatusViewModel(string deviceId, int mouldSlot, EMachineStatus status, int shiftNumber, DateTime date, DateTime timestamp)
    {
        DeviceId = deviceId;
        MouldSlot = mouldSlot;
        Status = status;
        ShiftNumber = shiftNumber;
        Date = date;
        Timestamp = timestamp;
    }
}
    
