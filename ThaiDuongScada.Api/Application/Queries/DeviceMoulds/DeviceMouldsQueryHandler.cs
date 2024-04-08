using Microsoft.EntityFrameworkCore;
using ThaiDuongScada.Infrastructure;

namespace ThaiDuongScada.Api.Application.Queries.DeviceMoulds;

public class DeviceMouldsQueryHandler : IRequestHandler<DeviceMouldsQuery, EShiftDuration>
{
    private readonly ApplicationDbContext _context;

    public DeviceMouldsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EShiftDuration> Handle(DeviceMouldsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.DeviceMoulds
            .Include(x => x.Mould)
            .AsNoTracking();

        if (request.DeviceType is not null)
        {
            queryable = queryable.Where(x => x.Device.DeviceType == request.DeviceType);
        }

        var deviceMoulds = await queryable.ToListAsync();

        return deviceMoulds.First().ShiftDuration;
    }
}
