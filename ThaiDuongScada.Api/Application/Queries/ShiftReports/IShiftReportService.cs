namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;

public interface IShiftReportService
{
    public Task<List<IEnumerable<ShiftReportViewModel>>> GetAllSecondLatestShiftReports();
}
