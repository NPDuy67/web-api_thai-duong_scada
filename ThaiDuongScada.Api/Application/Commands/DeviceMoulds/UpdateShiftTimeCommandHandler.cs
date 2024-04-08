using ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;

namespace ThaiDuongScada.Api.Application.Commands.DeviceMoulds;

public class UpdateShiftTimeCommandHandler : IRequestHandler<UpdateShiftTimeCommand, bool>
{
    private IDeviceMouldRepository _deviceMouldRepository;

    public UpdateShiftTimeCommandHandler(IDeviceMouldRepository deviceMouldRepository)
    {
        _deviceMouldRepository = deviceMouldRepository;
    }

    public async Task<bool> Handle(UpdateShiftTimeCommand command, CancellationToken cancellationToken)
    {
        var deviceMoulds = await _deviceMouldRepository.GetDeviceMouldByDeviceTypeAsync(command.DeviceType);

        foreach (var deviceMould in deviceMoulds)
        {
            deviceMould.ShiftDuration = command.ShiftDuration;
        }

        _deviceMouldRepository.UpdateRange(deviceMoulds);

        return await _deviceMouldRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
