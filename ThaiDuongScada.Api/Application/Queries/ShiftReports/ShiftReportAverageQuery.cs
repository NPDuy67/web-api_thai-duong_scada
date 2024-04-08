namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;

public class ShiftReportAverageQuery : IRequest<ShiftReportAverageViewModel>
{
    public DateTime StartTime { get; set; } = DateTime.MinValue;
    public DateTime EndTime { get; set; } = DateTime.Now;
}
 