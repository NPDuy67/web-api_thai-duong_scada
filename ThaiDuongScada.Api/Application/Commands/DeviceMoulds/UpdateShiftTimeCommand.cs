namespace ThaiDuongScada.Api.Application.Commands.DeviceMoulds;

public class UpdateShiftTimeCommand : IRequest<bool>
{
    public string DeviceType { get; set; }
    public EShiftDuration ShiftDuration { get; set; }

    public UpdateShiftTimeCommand(string deviceType, EShiftDuration shiftDuration)
    {
        DeviceType = deviceType;
        ShiftDuration = shiftDuration;
    }
}
