namespace ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
public interface IDeviceRepository : IRepository<Device>
{
    public Task<Device?> GetAsync(string deviceId);
    public Task<IEnumerable<Device>> GetByTypeAsync(string type);
    public Task<IEnumerable<Device>> GetAllDevice();
}
