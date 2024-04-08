using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;

namespace ThaiDuongScada.Host.Application.Dtos;
public class MessageType
{
    public EMessageType Value { get; }

    public MessageType(string deviceId, MetricMessage message)
    {
        if (message.Name == "machineStatus")
        {
            Value = EMessageType.MachineStatus;
        }
        else if (message.Name == "injectionCycle")
        {
            Value = EMessageType.InjectionCycle;
        }
        else if (message.Name == "injectionTime")
        {
            Value = EMessageType.InjectionTime;
        }
        else if (message.Name.StartsWith("badProduct"))
        {
            Value = EMessageType.DefectsCount;
        }
        else
        {
            Value = EMessageType.Unspecified;
        }
    }

    public enum EMessageType
    {
        MachineStatus,
        InjectionCycle,
        InjectionTime,
        DefectsCount,
        Unspecified
    }
}
