using System.Text.Json;
using ThaiDuongScada.Api.Application.Dtos;
namespace ThaiDuongScada.Api.Application.Workers;

public class Buffer
{
    private readonly List<TagChangedNotification> tagChangedNotifications = new List<TagChangedNotification>();

    public void Update(TagChangedNotification notification)
    {
        var existedNotification = tagChangedNotifications.FirstOrDefault(n => n.DeviceId == notification.DeviceId && n.TagId == notification.TagId);

        if (existedNotification is null)
        {
            tagChangedNotifications.Add(notification);
        }
        else
        {
            existedNotification.TagValue = notification.TagValue;
        }
    }

    public string GetAllTag() => JsonSerializer.Serialize(tagChangedNotifications);
}
