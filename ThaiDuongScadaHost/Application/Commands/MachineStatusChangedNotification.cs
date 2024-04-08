using ThaiDuongScada.Domain.AggregateModels.MachineStatusAggregate;
using ThaiDuongScada.Host.Application.Dtos;

namespace ThaiDuongScada.Host.Application.Commands;
public class MachineStatusChangedNotification : INotification
{
    public string DeviceId { get; set; }
    public EMachineStatus MachineStatus { get; set; }
    public DateTime Timestamp { get; set; }

    public MachineStatusChangedNotification(string deviceId, EMachineStatus machineStatus, DateTime timestamp)
    {
        DeviceId = deviceId;
        MachineStatus = machineStatus;
        Timestamp = timestamp;
    }
}