using ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;

namespace ThaiDuongScada.Infrastructure.Repositories;
public class DeviceMouldRepository : BaseRepository, IDeviceMouldRepository
{
    public DeviceMouldRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DeviceMould>> GetDeviceMouldsAsync(string deviceId)
    {
        return await _context.DeviceMoulds
            .Include(x => x.Mould)
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync();
    }

    public async Task<IEnumerable<DeviceMould>> GetAllDeviceMould()
    {
        return await _context.DeviceMoulds
                                .Include(p => p.Mould)
                                .ToListAsync();
    }

    public async Task<DeviceMould?> FindByIdAsync(string deviceId, int moudSlot)
    {
        return await _context.DeviceMoulds
            .Include(x => x.Mould)
            .FirstOrDefaultAsync(m => m.DeviceId == deviceId && m.MouldSlot == moudSlot);
    }

    public async Task<IEnumerable<DeviceMould>> GetDeviceMouldByDeviceTypeAsync(string deviceType)
    {
        return await _context.DeviceMoulds
                               .Include(p => p.Mould)
                               .Where(p => p.Device.DeviceType == deviceType)
                               .ToListAsync();
    }

    public void Update(DeviceMould deviceMould)
    {
        _context.DeviceMoulds.Update(deviceMould);
    }

    public void UpdateRange(IEnumerable<DeviceMould> deviceMoulds)
    {
        _context.DeviceMoulds.UpdateRange(deviceMoulds);
    }
}
