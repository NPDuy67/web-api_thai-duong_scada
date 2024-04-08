namespace ThaiDuongScada.Api.Application.Queries.DeviceMoulds;

public class DeviceMouldsQuery : IRequest<EShiftDuration>
{
    public string? DeviceType { get; set; }
}
