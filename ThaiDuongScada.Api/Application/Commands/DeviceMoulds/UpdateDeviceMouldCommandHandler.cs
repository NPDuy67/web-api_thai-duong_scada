using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ThaiDuongScada.Api.Application.Dtos;
using ThaiDuongScada.Api.Application.Hubs;
using ThaiDuongScada.Infrastructure;
using Buffer = ThaiDuongScada.Api.Application.Workers.Buffer;

namespace ThaiDuongScada.Api.Application.Commands.DeviceMoulds;
public class UpdateDeviceMouldCommandHandler : IRequestHandler<UpdateDeviceMouldCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly IDeviceMouldRepository _deviceMouldRepository;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly Buffer _buffer;

    public UpdateDeviceMouldCommandHandler(ApplicationDbContext context, IDeviceMouldRepository deviceMouldRepository, IHubContext<NotificationHub> hubContext, Buffer buffer)
    {
        _context = context;
        _deviceMouldRepository = deviceMouldRepository;
        _hubContext = hubContext;
        _buffer = buffer;
    }

    public async Task<bool> Handle(UpdateDeviceMouldCommand command, CancellationToken cancellationToken)
    {
        var existingDeviceMould = await _deviceMouldRepository.FindByIdAsync(command.DeviceId, command.MouldSlot);
        if (existingDeviceMould == null)
        {
            throw new Exception("Device not found");
        }

        var mould = await _context.Moulds.FirstOrDefaultAsync(x => x.MouldId == command.MouldId);
        if (mould is null)
        {
            throw new Exception("Mould not found");
        }

        existingDeviceMould.Mould = mould;
        existingDeviceMould.PreviousShotCount = command.PreviousShotCount;

        _deviceMouldRepository.Update(existingDeviceMould);

        var notification = new TagChangedNotification(command.DeviceId, $"products-{command.MouldSlot}", 0);
        _buffer.Update(notification);
        string json = JsonConvert.SerializeObject(notification);
        await _hubContext.Clients.All.SendAsync("TagChanged", json);

        return await _deviceMouldRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
