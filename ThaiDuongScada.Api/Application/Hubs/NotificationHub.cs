using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ThaiDuongScada.Api.Application.Dtos;
using ThaiDuongScada.Infrastructure.Communication;
using Buffer = ThaiDuongScada.Api.Application.Workers.Buffer;

namespace ThaiDuongScada.Api.Application.Hubs;

public class NotificationHub: Hub
{
    private readonly Buffer _buffer;
    private readonly ManagedMqttClient _mqttClient;

    public NotificationHub(Buffer buffer, ManagedMqttClient mqttClient)
    {
        _buffer = buffer;
        _mqttClient = mqttClient;
    }

    public string SendAll()
    {
        string allTags = _buffer.GetAllTag();
        return allTags;
    }

    public async Task SendAllTag()
    {
        string allTags = _buffer.GetAllTag();

        await Clients.All.SendAsync("GetAll", allTags);
    }

    public async Task SendCommand(string deviceId, int command)
    {
        string topic = $"IMM/{deviceId}/CollectingData";

        var payload = new SendCommand(DateTime.UtcNow, command);
        var payloadMessage = JsonConvert.SerializeObject(payload);

        await _mqttClient.Publish(topic, payloadMessage, true);
    }
}
