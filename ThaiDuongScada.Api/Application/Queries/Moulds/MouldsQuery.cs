namespace ThaiDuongScada.Api.Application.Queries.Moulds;
public class MouldsQuery : IRequest<IEnumerable<MouldViewModel>>
{
    public string? DeviceType { get; set; }
}
