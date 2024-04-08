using System.Data;
using ThaiDuongScada.Domain.AggregateModels.MachineStatusAggregate;

namespace ThaiDuongScada.Infrastructure.Repositories;
public class MachineStatusRepository : BaseRepository, IMachineStatusRepository
{
    public MachineStatusRepository(ApplicationDbContext context) : base(context)
    {

    }
    
    public async Task AddAsync(MachineStatus machineStatus)
    {
        await _context.MachineStatus.AddAsync(machineStatus);
    }

    public async Task<bool> ExistsAsync(string deviceId, int mouldSlot, DateTime timestamp)
    {
        return await _context.MachineStatus
            .AnyAsync(x => x.DeviceId == deviceId
                      && x.MouldSlot == mouldSlot
                      && x.Timestamp == timestamp);
    }

    public async Task<MachineStatus?> GetLatestAsync(string deviceId, int mouldSlot)
    {
        return await _context.MachineStatus
            .Where(x => x.DeviceId == deviceId && x.MouldSlot == mouldSlot)
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync();
    }
}