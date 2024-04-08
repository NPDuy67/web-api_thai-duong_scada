using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiDuongScada.Infrastructure;

namespace ThaiDuongScada.Api.Application.Queries.MachineStatus;

public class MachineStatusQueryHandler : IRequestHandler<MachineStatusQuery, IEnumerable<MachineStatusViewModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MachineStatusQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MachineStatusViewModel>> Handle(MachineStatusQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.MachineStatus.AsNoTracking();

        if (request.DeviceId is not null)
        {
            queryable = queryable.Where(x => x.DeviceId == request.DeviceId 
                                            && x.MouldSlot == request.MouldSlot
                                            && x.Date >= request.StartTime
                                            && x.Date <= request.EndTime)
                                 .OrderByDescending(x => x.Timestamp);
        }

        var machineStatus = await queryable.ToListAsync();

        return _mapper.Map<IEnumerable<MachineStatusViewModel>>(machineStatus);
    }
}
