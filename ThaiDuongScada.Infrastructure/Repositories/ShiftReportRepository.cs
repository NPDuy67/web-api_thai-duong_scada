using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
using static System.Net.Mime.MediaTypeNames;

namespace ThaiDuongScada.Infrastructure.Repositories;
public class ShiftReportRepository : BaseRepository, IShiftReportRepository
{
    public ShiftReportRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task AddAsync(ShiftReport shiftReport)
    {
        if (!await ExistsAsync(shiftReport.DeviceId, shiftReport.MouldSlot, shiftReport.ShiftNumber, shiftReport.Date))
        {
            await _context.ShiftReports.AddAsync(shiftReport);
        }
    }

    public async Task<bool> ExistsAsync(string deviceId, int mouldSlot, int shiftNumber, DateTime date)
    {
        return await _context.ShiftReports
            .AnyAsync(x => x.DeviceId == deviceId
                           && x.MouldSlot == mouldSlot
                           && x.ShiftNumber == shiftNumber
                           && x.Date == date);
    }

    public async Task<ShiftReport?> GetAsync(string deviceId, int mouldSlot, int shiftNumber, DateTime date)
    {
        return await _context.ShiftReports
            .Include(x => x.Shots)
            .Include(x => x.Device)
            .FirstOrDefaultAsync(x => x.DeviceId == deviceId && x.MouldSlot == mouldSlot && x.ShiftNumber == shiftNumber && x.Date == date);
    }

    public async Task<IEnumerable<ShiftReport>> GetAllSecondLatestShiftReports()
    {
        var shiftInfos = await _context.ShiftReports
            .GroupBy(c => new ShiftInfo(c.Date, c.ShiftNumber))
            .Select(c => c.Key)
            .ToListAsync();

        var secondLatestShiftReport = shiftInfos
            .OrderByDescending(c => c.Date)
            .ThenByDescending(c => c.ShiftNumber)
            .Skip(1).FirstOrDefault();

        if (secondLatestShiftReport is null)
        {
            throw new Exception("SecondLastestShiftReports not found");
        }
        return await _context.ShiftReports
                .Where(c => c.Date == secondLatestShiftReport.Date)
                .Where(c => c.ShiftNumber == secondLatestShiftReport.ShiftNumber)
                .ToListAsync();
    }

    private class ShiftInfo
    {
        public DateTime Date { get; set; }
        public int ShiftNumber { get; set; }

        public ShiftInfo(DateTime date, int shiftNumber)
        {
            Date = date;
            ShiftNumber = shiftNumber;
        }
    }
}
