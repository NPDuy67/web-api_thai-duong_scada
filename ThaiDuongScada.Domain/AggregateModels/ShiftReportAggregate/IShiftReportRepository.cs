namespace ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
public interface IShiftReportRepository: IRepository<ShiftReport>
{
    public Task AddAsync(ShiftReport shiftReport);
    public Task<ShiftReport?> GetAsync(string deviceId, int mouldSlot, int shiftNumber, DateTime date);
    public Task<bool> ExistsAsync(string deviceId, int mouldSlot, int shiftNumber, DateTime date);
    public Task<IEnumerable<ShiftReport>> GetAllSecondLatestShiftReports();
}
