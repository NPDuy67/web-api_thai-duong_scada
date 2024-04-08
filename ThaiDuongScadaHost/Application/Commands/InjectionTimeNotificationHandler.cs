using ThaiDuongScada.Host.Application.Stores;

namespace ThaiDuongScada.Host.Application.Commands;
public class InjectionTimeNotificationHandler : INotificationHandler<InjectionTimeNotification>
{
    private readonly InjectionTimeBuffers _injectionTimeBuffers;

    public InjectionTimeNotificationHandler(InjectionTimeBuffers injectionTimeBuffers)
    {
        _injectionTimeBuffers = injectionTimeBuffers;
    }

    public Task Handle(InjectionTimeNotification notification, CancellationToken cancellationToken)
    {
        _injectionTimeBuffers.Update(notification.DeviceId, notification.InjectionTime);
        return Task.CompletedTask;
    }
}
