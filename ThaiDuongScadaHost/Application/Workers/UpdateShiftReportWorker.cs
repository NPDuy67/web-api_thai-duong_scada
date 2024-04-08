using MediatR;
using Newtonsoft.Json;
using ThaiDuongScada.Domain.AggregateModels.MachineStatusAggregate;
using ThaiDuongScada.Host.Application.Commands;
using ThaiDuongScada.Host.Application.Dtos;
using ThaiDuongScada.Infrastructure.Communication;

namespace ThaiDuongScada.Host.Application.Workers;
public class UpdateShiftReportWorker : BackgroundService
{
    private readonly ManagedMqttClient _mqttClient;
    private readonly IServiceScopeFactory _scopeFactory;

    public UpdateShiftReportWorker(ManagedMqttClient mqttClient, IServiceScopeFactory scopeFactory)
    {
        _mqttClient = mqttClient;
        _scopeFactory = scopeFactory;
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
        await _mqttClient.Subscribe("IMM/+/LWT");
    }

    private async Task OnMqttClientMessageReceivedAsync(MqttMessage e)
    {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var topic = e.Topic;
        var payloadMessage = e.Payload;
        if (topic is null || payloadMessage is null)
        {
            return;
        }

        var topicSegments = topic.Split('/');
        var deviceId = topicSegments[1];

        if (topicSegments[2] == "LWT")
        {
            var notification = new LastWillMessageNotification(deviceId, payloadMessage);
            await mediator.Publish(notification);
        }
        else
        {
            var metrics = JsonConvert.DeserializeObject<List<MetricMessage>>(payloadMessage);
            if (metrics is null)
            {
                return;
            }
            foreach (var metric in metrics)
            {
                var messageType = new MessageType(deviceId, metric);
                switch (messageType.Value)
                {
                    case MessageType.EMessageType.MachineStatus:
                        var statusCode = (long)metric.Value;
                        var machineStatus = (EMachineStatus)statusCode;
                        var machineStatusNotification = new MachineStatusChangedNotification(deviceId, machineStatus, metric.Timestamp);
                        await mediator.Publish(machineStatusNotification);
                        break;
                    case MessageType.EMessageType.InjectionCycle:
                        var injectionCycle = (double)metric.Value;
                        var injectionCycleNotification = new InjectionCycleNotification(deviceId, injectionCycle, metric.Timestamp);
                        await mediator.Publish(injectionCycleNotification);
                        break;
                    case MessageType.EMessageType.InjectionTime:
                        var injectionTime = (double)metric.Value;
                        var injectionTimeNotification = new InjectionTimeNotification(deviceId, injectionTime);
                        await mediator.Publish(injectionTimeNotification);
                        break;
                    case MessageType.EMessageType.DefectsCount:
                        var defectsCount = Convert.ToInt32(metric.Value);
                        var nameSegments = metric.Name.Split('-');
                        int mouldSlot = nameSegments.Length > 1 ? int.Parse(nameSegments[1]) : 1;
                        var defectCountNotification = new DefectCountNotification(deviceId, mouldSlot, defectsCount, metric.Timestamp);
                        await mediator.Publish(defectCountNotification);
                        break;
                }
            }
        }
    }
}
