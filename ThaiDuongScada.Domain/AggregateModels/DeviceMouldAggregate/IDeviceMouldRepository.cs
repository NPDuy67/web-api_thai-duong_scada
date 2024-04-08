namespace ThaiDuongScada.Domain.AggregateModels.DeviceMouldAggregate;
public interface IDeviceMouldRepository : IRepository<DeviceMould>
{
    public Task<IEnumerable<DeviceMould>> GetDeviceMouldsAsync(string deviceId);
    public Task<IEnumerable<DeviceMould>> GetAllDeviceMould();
    public Task<DeviceMould?> FindByIdAsync(string deviceId, int moudSlot);
    public Task<IEnumerable<DeviceMould>> GetDeviceMouldByDeviceTypeAsync(string deviceType);
    public void Update(DeviceMould deviceMould);
    public void UpdateRange(IEnumerable<DeviceMould> deviceMoulds);
}
