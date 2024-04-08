using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using ThaiDuongScada.Api.Application.Dtos;
using ThaiDuongScada.Api.Application.Hubs;
using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
using ThaiDuongScada.Infrastructure.Communication;

namespace ThaiDuongScada.Api.Application.Workers;

public class ScadaHost : BackgroundService
{
    private readonly ManagedMqttClient _mqttClient;
    private readonly Buffer _buffer;
    private readonly IHubContext<NotificationHub> _hubContext;

    public ScadaHost(ManagedMqttClient mqttClient, Buffer buffer, IHubContext<NotificationHub> hubContext)
    {
        _mqttClient = mqttClient;
        _buffer = buffer;
        _hubContext = hubContext;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConnectToMqttBrokerAsync();
    }

    private async Task ConnectToMqttBrokerAsync()
    {
        _mqttClient.MessageReceived += OnMqttClientMessageReceivedAsync;
        await _mqttClient.ConnectAsync();

        await _mqttClient.Subscribe("IMM/+/Metric");
        await _mqttClient.Subscribe("IMM/+/Metric/+");
        await _mqttClient.Subscribe("PS/+/+");
    }

    private async Task OnMqttClientMessageReceivedAsync(MqttMessage e)
    {
        var topic = e.Topic;
        var payloadMessage = e.Payload;
        if (topic is null || payloadMessage is null)
        {
            return;
        }

        var topicSegments = topic.Split('/');
        var deviceId = topicSegments[1];

        var metrics = DeserializeObject(topicSegments, payloadMessage);
        if (metrics is null)
        {
            return;
        }
        foreach (var metric in metrics)
        {
            var notification = new TagChangedNotification(deviceId, metric.Name, metric.Value);
            _buffer.Update(notification);
            string json = JsonConvert.SerializeObject(notification);
            await _hubContext.Clients.All.SendAsync("TagChanged", json);
        }
    }

    private List<MetricMessage>? DeserializeObject(string[] topicSegments, string payload)
    {
        var metrics = new List<MetricMessage>();

        if (topicSegments[0] == "PS")
        {
            var metric = JObject.Parse(payload);

            var timestamp = (DateTime)metric["Timestamp"];
            switch (topicSegments[2])
            {
                case "ErrorMessage":
                    var nameEvent = (string)metric["NameEvent"];
                    metrics.Add(new MetricMessage("ErrorMessage", nameEvent, timestamp));
                    break;
                case "ValueMessage":
                    var metricNames = new string[]
                    {
                            "ItemId",
                            "CompletedProduct",
                            "ErrorProduct",
                            "WorkingTime",
                            "ExecutionTime",
                            "SumActualPro"
                    };

                    foreach (var name in metricNames)
                    {
                        object value;
                        if (name == "WorkingTime")
                        {
                            value = (dynamic)metric[name] * 60;
                        }
                        else
                        {
                            value = (dynamic)metric[name];
                        }
                        metrics.Add(new MetricMessage(name, value, timestamp));
                    }
                    break;
                case "MachineStatus":
                    var machineStatus = (string)metric["MachineStatus"];
                    metrics.Add(new MetricMessage("MachineStatus", machineStatus, timestamp));
                    break;
            }
        }

        else
        {
            metrics = JsonConvert.DeserializeObject<List<MetricMessage>>(payload);
        }

        return metrics;
    }
}
