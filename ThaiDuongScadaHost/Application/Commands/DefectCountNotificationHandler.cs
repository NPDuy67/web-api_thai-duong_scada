using ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
using ThaiDuongScada.Host.Application.Services;

namespace ThaiDuongScada.Host.Application.Commands;
public class DefectCountNotificationHandler : INotificationHandler<DefectCountNotification>
{
    private readonly IDeviceMouldRepository _deviceMouldRepository;
    private readonly IShiftReportRepository _shiftReportRepository;
    private readonly MetricMessagePublisher _metricMessagePublisher;

    public DefectCountNotificationHandler(IDeviceMouldRepository deviceMouldRepository, IShiftReportRepository shiftReportRepository, MetricMessagePublisher metricMessagePublisher)
    {
        _deviceMouldRepository = deviceMouldRepository;
        _shiftReportRepository = shiftReportRepository;
        _metricMessagePublisher = metricMessagePublisher;
    }
    
    public async Task Handle(DefectCountNotification notification, CancellationToken cancellationToken)
    {
        var deviceMoulds = await _deviceMouldRepository.GetDeviceMouldsAsync(notification.DeviceId);
        var deviceMould = deviceMoulds.FirstOrDefault(x => x.MouldSlot == notification.MouldSlot);

        if (deviceMould is null)
        {
            return;
        }

        var shiftNumber = ShiftTimeHelper.GetShiftNumber(notification.Timestamp, deviceMould.ShiftDuration);
        var shiftDate = ShiftTimeHelper.GetShiftDate(notification.Timestamp, deviceMould.ShiftDuration);
        var shiftReport = await _shiftReportRepository.GetAsync(notification.DeviceId, deviceMould.MouldSlot, shiftNumber, shiftDate);
        if (shiftReport is null)
        {
            return;
        }

        shiftReport.SetDefectCount(notification.DefectCount);

        await _shiftReportRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        if (notification.DefectCount > 0)
        {
            await _metricMessagePublisher.PublishMetricMessage(notification.DeviceId, $"Q-{deviceMould.MouldSlot}", shiftReport.Q, notification.Timestamp);
            await _metricMessagePublisher.PublishMetricMessage(notification.DeviceId, $"OEE-{deviceMould.MouldSlot}", shiftReport.OEE, notification.Timestamp);
        }     
    }
}
