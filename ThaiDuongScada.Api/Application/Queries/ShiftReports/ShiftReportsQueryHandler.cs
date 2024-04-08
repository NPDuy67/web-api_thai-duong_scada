using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using ThaiDuongScada.Infrastructure;

namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;

public class ShiftReportsQueryHandler : IRequestHandler<ShiftReportsQuery, IEnumerable<ShiftReportViewModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ShiftReportsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShiftReportViewModel>> Handle(ShiftReportsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.ShiftReports
            .Include(x => x.Shots)
            .AsNoTracking();

        if(request.DeviceId is not null)
        {
            queryable = queryable.Where(x => x.DeviceId == request.DeviceId
                                            && x.Date >= request.StartTime
                                            && x.Date <= request.EndTime
                                            && x.MouldSlot == request.MouldSlot);
        }
        var shiftReports = await queryable.ToListAsync();

        return _mapper.Map<IEnumerable<ShiftReport>, IEnumerable<ShiftReportViewModel>>(shiftReports);
    }
}
