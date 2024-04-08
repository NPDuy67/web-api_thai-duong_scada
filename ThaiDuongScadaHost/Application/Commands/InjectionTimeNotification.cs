namespace ThaiDuongScada.Host.Application.Commands;
public class InjectionTimeNotification: INotification
{
    public string DeviceId { get; set; }
    public double InjectionTime { get; set; }

    public InjectionTimeNotification(string deviceId, double injectionTime)
    {
        DeviceId = deviceId;
        InjectionTime = injectionTime;
    }
}
