using ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;

namespace ThaiDuongScada.Infrastructure.Repositories;
public class DeviceRepository : BaseRepository, IDeviceRepository
{
    public DeviceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Device?> GetAsync(string deviceId)
    {
        return await _context.Devices
            .FirstOrDefaultAsync(x => x.DeviceId == deviceId);
    }

    public async Task<IEnumerable<Device>> GetByTypeAsync(string type)
    {
        var devices = await _context.Devices
             .Where(c => c.DeviceType == type)
             .ToListAsync();
        return devices;
    }

    public async Task<IEnumerable<Device>> GetAllDevice()
    {
        return await _context.Devices.ToListAsync();
    }
}
