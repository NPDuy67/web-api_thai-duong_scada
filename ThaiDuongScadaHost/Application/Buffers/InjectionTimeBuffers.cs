namespace ThaiDuongScada.Host.Application.Stores;
public class InjectionTimeBuffers
{
    private readonly Dictionary<string, double> deviceLatestInjectionTimes = new();

    public void Update(string deviceId, double injectionTime)
    {
        deviceLatestInjectionTimes[deviceId] = injectionTime;
    }

    public double GetDeviceLatestInjectionTime(string deviceId)
    {
        return deviceLatestInjectionTimes[deviceId];
    }
}
