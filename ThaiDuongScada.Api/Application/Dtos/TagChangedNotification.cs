namespace ThaiDuongScada.Api.Application.Dtos;

public class TagChangedNotification
{
    public string DeviceId { get; set; }
    public string TagId { get; set; }
    public object TagValue { get; set; }

    public TagChangedNotification(string deviceId, string tagId, object tagValue)
    {
        DeviceId = deviceId;
        TagId = tagId;
        TagValue = tagValue;
    }
}
