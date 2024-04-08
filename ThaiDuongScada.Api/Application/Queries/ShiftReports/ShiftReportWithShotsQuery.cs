namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;

public class ShiftReportWithShotsQuery : IRequest<IEnumerable<ShiftReportWithShotViewModel>>
{
    public int ShiftId { get; set; }
}
