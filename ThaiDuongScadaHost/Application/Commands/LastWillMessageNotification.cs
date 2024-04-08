namespace ThaiDuongScada.Host.Application.Commands;
public class LastWillMessageNotification: INotification
{
    public string DeviceId { get; set; }
    public string Payload { get; set; }

    public LastWillMessageNotification(string deviceId, string payload)
    {
        DeviceId = deviceId;
        Payload = payload;
    }
}
