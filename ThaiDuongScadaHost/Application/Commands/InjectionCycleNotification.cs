namespace ThaiDuongScada.Host.Application.Commands;
public class InjectionCycleNotification: INotification
{
    public string DeviceId { get; set; }
    public double InjectionCycle { get; set; }
    public DateTime Timestamp { get; set; }

    public InjectionCycleNotification(string deviceId, double injectionCycle, DateTime timestamp)
    {
        DeviceId = deviceId;
        InjectionCycle = injectionCycle;
        Timestamp = timestamp;
    }
}
