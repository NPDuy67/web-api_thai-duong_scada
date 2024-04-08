using MediatR;
using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
using ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;
using ThaiDuongScada.Domain.AggregateModels.MachineStatusAggregate;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
using ThaiDuongScada.Host.Application.Services;
using ThaiDuongScada.Host.Application.Stores;
using ThaiDuongScada.Infrastructure.Communication;
using ThaiDuongScada.Infrastructure.Repositories;

namespace ThaiDuongScada.Host.Application.Commands;
public class MachineStatusChangedNotificationHandler : INotificationHandler<MachineStatusChangedNotification>
{
    private readonly ManagedMqttClient _mqttClient;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceMouldRepository _deviceMouldRepository;
    private readonly IMachineStatusRepository _machineStatusRepository;
    private readonly MetricMessagePublisher _metricMessagePublisher;

    public MachineStatusChangedNotificationHandler(ManagedMqttClient mqttClient, IDeviceRepository deviceRepository, IDeviceMouldRepository deviceMouldRepository, IMachineStatusRepository machineStatusRepository, MetricMessagePublisher metricMessagePublisher)
    {
        _mqttClient = mqttClient;
        _deviceRepository = deviceRepository;
        _deviceMouldRepository = deviceMouldRepository;
        _machineStatusRepository = machineStatusRepository;
        _metricMessagePublisher = metricMessagePublisher;
    }

    public async Task Handle(MachineStatusChangedNotification notification, CancellationToken cancellationToken)
    {
        if (notification.MachineStatus == EMachineStatus.Disconnected)
        {
            await ClearDataDevices(notification.DeviceId, notification.Timestamp);
        }

        var device = await _deviceRepository.GetAsync(notification.DeviceId);
        if (device is null)
        {
            return;
        }

        var deviceMoulds = await _deviceMouldRepository.GetDeviceMouldsAsync(notification.DeviceId);

        foreach (var deviceMould in deviceMoulds)
        {
            var machineStatus = new MachineStatus(deviceMould.MouldSlot, device, deviceMould.ShiftDuration, notification.MachineStatus, notification.Timestamp);
            if (!await _machineStatusRepository.ExistsAsync(notification.DeviceId, deviceMould.MouldSlot, notification.Timestamp))
            {
                var latestStatus = await _machineStatusRepository.GetLatestAsync(notification.DeviceId, deviceMould.MouldSlot);
                
                if (latestStatus is null || notification.MachineStatus != latestStatus.Status)
                {
                    await _machineStatusRepository.AddAsync(machineStatus);
                }
            }
        }

        await _machineStatusRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }

    private async Task ClearDataDevices(string deviceId, DateTime timestamp)
    {
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "Switch Over Pos", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "tmChargeTime", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "tmClpClsTime", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "injectionCycle", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "injectionTime", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "tmInjPosi1", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "tmInj2HoldTime", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "Injection Peak Pressure", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "Peak Injection Speed", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "Nozzle Temp", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "Injection Peak Pressure", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "tmTemp1_Set", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "tmClpOpnTime", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "tmInjBackTime", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "tmCoolingTime", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "temperature", 0, timestamp);
        await _metricMessagePublisher.PublishMetricMessage(deviceId, "pressure", 0, timestamp);

        await _mqttClient.Publish($"IMM/{deviceId}/Metric/Switch Over Pos", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/tmChargeTime", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/tmClpClsTime", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/injectionCycle", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/injectionTime", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/tmInjPosi1", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/tmInj2HoldTime", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/Injection Peak Pressure", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/Peak Injection Speed", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/tmTemp1_Set", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/tmClpOpnTime", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/tmInjBackTime", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/tmCoolingTime", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/temperature", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/pressure", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/badProduct-1", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/badProduct-2", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/operationTime", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/badProductFront", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/badProductBack", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/TotalInjectionCycle", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/counterShot", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/shiftNumber", "", true);
        await _mqttClient.Publish($"IMM/{deviceId}/Metric/simulated", "", false);
    }
}
