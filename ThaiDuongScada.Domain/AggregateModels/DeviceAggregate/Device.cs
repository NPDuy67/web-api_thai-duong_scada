namespace ThaiDuongScada.Domain.AggregateModels.DeviceAggregate;
public class Device: IAggregateRoot
{
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public int DisplayPriority { get; set; }
    public string DeviceType { get; set; }
    public int MinTemperature { get; set; }
    public int MaxTemperature { get; set; }
    public double MinPressure { get; set; }
    public double MaxPressure { get; set; }
    public bool Manipular { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Device() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Device(string deviceId, string deviceName, int displayPriority, string deviceType, int minTemperature, int maxTemperature, double minPressure, double maxPressure, bool manipular)
    {
        DeviceId = deviceId;
        DeviceName = deviceName;
        DisplayPriority = displayPriority;
        DeviceType = deviceType;
        MinTemperature = minTemperature;
        MaxTemperature = maxTemperature;
        MinPressure = minPressure;
        MaxPressure = maxPressure;
        Manipular = manipular;
    }
}
