namespace ThaiDuongScada.Api.Application.Queries.Devices;
public class DevicesQuery : IRequest<IEnumerable<DeviceViewModel>>
{
    public string? DeviceType { get; set; }
}
