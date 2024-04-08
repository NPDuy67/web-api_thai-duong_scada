using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiDuongScada.Domain.AggregateModels.MouldAggregate;
using ThaiDuongScada.Infrastructure;
namespace ThaiDuongScada.Api.Application.Queries.Moulds;

public class MouldsQueryHandler : IRequestHandler<MouldsQuery, IEnumerable<MouldViewModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MouldsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MouldViewModel>> Handle(MouldsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.Moulds.AsNoTracking();

        if (request.DeviceType is not null)
        {
            queryable = queryable.Where(x => x.DeviceType == request.DeviceType);
        }

        var moulds = await queryable.ToListAsync();

        return _mapper.Map<IEnumerable<Mould>, IEnumerable<MouldViewModel>>(moulds);
    }
}
