using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiDuongScada.Infrastructure;

namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;

public class ShiftReportWithShotsQueryHandler : IRequestHandler<ShiftReportWithShotsQuery, IEnumerable<ShiftReportWithShotViewModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ShiftReportWithShotsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShiftReportWithShotViewModel>> Handle(ShiftReportWithShotsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.ShiftReports
            .Include(x => x.Shots)
            .AsNoTracking();

        if (request.ShiftId != 0)
        {
            queryable = queryable.Where(x => x.Id == request.ShiftId);
        }
        var shiftReports = await queryable.ToListAsync();

        return _mapper.Map<IEnumerable<ShiftReport>, IEnumerable<ShiftReportWithShotViewModel>>(shiftReports);
    }
}
