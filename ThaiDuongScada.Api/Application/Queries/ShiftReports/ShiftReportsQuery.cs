namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;

public class ShiftReportsQuery : IRequest<IEnumerable<ShiftReportViewModel>>
{
    public string DeviceId { get; set; } = "";
    public int MouldSlot { get; set; } = 1;
    public DateTime StartTime { get; set; } = DateTime.MinValue;
    public DateTime EndTime { get; set; } = DateTime.Now;
}
