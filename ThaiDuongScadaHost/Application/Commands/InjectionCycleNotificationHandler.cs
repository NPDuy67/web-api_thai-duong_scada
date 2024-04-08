using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
using ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
using ThaiDuongScada.Host.Application.Services;
using ThaiDuongScada.Host.Application.Stores;

namespace ThaiDuongScada.Host.Application.Commands;
public class InjectionCycleNotificationHandler : INotificationHandler<InjectionCycleNotification>
{
    private readonly InjectionTimeBuffers _injectionTimeBuffers;
    private readonly IShiftReportRepository _shiftReportRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceMouldRepository _deviceMouldRepository;
    private readonly MetricMessagePublisher _metricMessagePublisher;

    public InjectionCycleNotificationHandler(InjectionTimeBuffers injectionTimeBuffers, IShiftReportRepository shiftReportRepository, IDeviceRepository deviceRepository, IDeviceMouldRepository deviceMouldRepository, MetricMessagePublisher metricMessagePublisher)
    {
        _injectionTimeBuffers = injectionTimeBuffers;
        _shiftReportRepository = shiftReportRepository;
        _deviceRepository = deviceRepository;
        _deviceMouldRepository = deviceMouldRepository;
        _metricMessagePublisher = metricMessagePublisher;
    }

    public async Task Handle(InjectionCycleNotification notification, CancellationToken cancellationToken)
    {
        var device = await _deviceRepository.GetAsync(notification.DeviceId);
        if (device is null)
        {
            return;
        }

        var deviceMoulds = await _deviceMouldRepository.GetDeviceMouldsAsync(notification.DeviceId);

        foreach (var deviceMould in deviceMoulds)
        {
            var shiftReport = new ShiftReport(deviceMould.MouldSlot, device, deviceMould.ShiftDuration, notification.Timestamp);

            var existingShiftReport = await _shiftReportRepository.GetAsync(device.DeviceId, deviceMould.MouldSlot, shiftReport.ShiftNumber, shiftReport.Date);
            if (existingShiftReport is not null)
            {
                shiftReport = existingShiftReport;
            }
            else
            {
                deviceMould.SetPreviousShotCount(0);
                await _shiftReportRepository.AddAsync(shiftReport);
            }

            var injectionTime = _injectionTimeBuffers.GetDeviceLatestInjectionTime(notification.DeviceId);
            shiftReport.AddShot(injectionTime, notification.InjectionCycle, notification.Timestamp);
            shiftReport.SetProductCount(deviceMould.Mould.Cav * (shiftReport.Shots.Count - deviceMould.PreviousShotCount));
            shiftReport.SetProductPercentage((double)(shiftReport.ProductCount) / (double)deviceMould.Mould.StandardOutput);

            await _shiftReportRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            await _metricMessagePublisher.PublishMetricMessage(device.DeviceId, $"A-{deviceMould.MouldSlot}", shiftReport.A, notification.Timestamp);
            await _metricMessagePublisher.PublishMetricMessage(device.DeviceId, $"P-{deviceMould.MouldSlot}", shiftReport.P, notification.Timestamp);
            await _metricMessagePublisher.PublishMetricMessage(device.DeviceId, $"Q-{deviceMould.MouldSlot}", shiftReport.Q, notification.Timestamp);
            await _metricMessagePublisher.PublishMetricMessage(device.DeviceId, $"L-{deviceMould.MouldSlot}", shiftReport.L, notification.Timestamp);
            await _metricMessagePublisher.PublishMetricMessage(device.DeviceId, $"OEE-{deviceMould.MouldSlot}", shiftReport.OEE, notification.Timestamp);
            await _metricMessagePublisher.PublishMetricMessage(device.DeviceId, $"productPercentage-{deviceMould.MouldSlot}", shiftReport.ProductPercentage * 100, notification.Timestamp);
            await _metricMessagePublisher.PublishMetricMessage(device.DeviceId, $"products-{deviceMould.MouldSlot}", shiftReport.ProductCount, notification.Timestamp);
            await _metricMessagePublisher.PublishMetricMessage(device.DeviceId, "counterShot", shiftReport.Shots.Count, notification.Timestamp);
            await _metricMessagePublisher.PublishMetricMessage(device.DeviceId, "operationTime", shiftReport.TotalInjectionTime, notification.Timestamp);
        }
    }
}
