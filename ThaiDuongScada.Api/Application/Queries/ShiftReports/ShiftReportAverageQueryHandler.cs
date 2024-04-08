using Microsoft.EntityFrameworkCore;
using ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
using ThaiDuongScada.Infrastructure;

namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;

public class ShiftReportAverageQueryHandler : IRequestHandler<ShiftReportAverageQuery, ShiftReportAverageViewModel>
{
    private readonly ApplicationDbContext _context;

    public ShiftReportAverageQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShiftReportAverageViewModel> Handle(ShiftReportAverageQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.ShiftReports.AsNoTracking();
            
        queryable = queryable.Where(x =>
                        x.Date.CompareTo(request.StartTime) >= 0 &&
                        x.Date.CompareTo(request.EndTime) <= 0);


        var shiftReports = await queryable.ToListAsync();

        double averageOEE = shiftReports.Average(x => x.OEE);
        double averageA = shiftReports.Average(x => x.A);
        double averageP = shiftReports.Average(x => x.P);
        double averageQ = shiftReports.Average(x => x.Q);

        var shiftReportAverage = new ShiftReportAverageViewModel(averageOEE, averageP, averageA, averageQ);

        return shiftReportAverage;
    }
}
