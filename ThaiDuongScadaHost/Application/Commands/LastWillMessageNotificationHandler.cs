using Newtonsoft.Json;
using ThaiDuongScada.Host.Application.Dtos;
using ThaiDuongScada.Infrastructure.Communication;

namespace ThaiDuongScada.Host.Application.Commands;
public class LastWillMessageNotificationHandler : INotificationHandler<LastWillMessageNotification>
{
    private readonly ManagedMqttClient _mqttClient;

    public LastWillMessageNotificationHandler(ManagedMqttClient mqttClient)
    {
        _mqttClient = mqttClient;
    }

    public async Task Handle(LastWillMessageNotification notification, CancellationToken cancellationToken)
    {
        notification.Payload = notification.Payload.Replace("\\", "");
        notification.Payload = notification.Payload.Replace("\r", "");
        notification.Payload = notification.Payload.Replace("\n", "");
        notification.Payload = notification.Payload.Replace(" ", "");

        var lwtMessage = JsonConvert.DeserializeObject<LastWillMessage>(notification.Payload);

        if (lwtMessage is not null)
        {
            foreach (var machines in lwtMessage.Machines)
            {
                var statusMetric = new MetricMessage("machineStatus", 5, DateTime.Now.ToUniversalTime().AddDays(7));
                var statusJson = JsonConvert.SerializeObject(new List<MetricMessage>() { statusMetric });
                await _mqttClient.Publish($"IMM/{machines.MachineId}/Metric/machineStatus", statusJson, true);
            }
        }
    }
}
