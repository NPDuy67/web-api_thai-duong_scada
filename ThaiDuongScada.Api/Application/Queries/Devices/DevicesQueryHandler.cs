using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ThaiDuongScada.Api.Application.Queries.DeviceMoulds;
using ThaiDuongScada.Api.Application.Queries.ShiftReports;
using ThaiDuongScada.Infrastructure;

namespace ThaiDuongScada.Api.Application.Queries.Devices;
public class DevicesQueryHandler : IRequestHandler<DevicesQuery, IEnumerable<DeviceViewModel>>
{
    private readonly ApplicationDbContext _context;
    private readonly IShiftReportRepository _shiftReportRepository;
    private readonly IDeviceMouldRepository _deviceMouldRepository;
    private readonly IMapper _mapper;

    public DevicesQueryHandler(ApplicationDbContext context, IShiftReportRepository shiftReportRepository, IDeviceMouldRepository deviceMouldRepository, IMapper mapper)
    {
        _context = context;
        _shiftReportRepository = shiftReportRepository;
        _deviceMouldRepository = deviceMouldRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DeviceViewModel>> Handle(DevicesQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.Devices.AsNoTracking();

        if (request.DeviceType is not null)
        {
            queryable = queryable.Where(x => x.DeviceType == request.DeviceType);
        }

        var devices = await queryable.ToListAsync();
        var deviceViewModels = _mapper.Map<IEnumerable<Device>, IEnumerable<DeviceViewModel>>(devices);

        var allSecondLastestShiftReports = (await _shiftReportRepository.GetAllSecondLatestShiftReports()).ToList();
        var deviceMoulds = (await _deviceMouldRepository.GetAllDeviceMould()).ToList();

        foreach (var viewModel in deviceViewModels)
        {
            string deviceId = viewModel.DeviceId;

            var preShiftReports = (allSecondLastestShiftReports.FindAll(g => g.DeviceId == deviceId)).AsEnumerable();

            if (preShiftReports != null)
            {
                var preShiftReportViewModels = _mapper.Map<IEnumerable<ShiftReport>, IEnumerable<ShiftReportViewModel>>(preShiftReports);
                foreach (var report in preShiftReportViewModels)
                {
                    report.ProductPercentage *= 100;
                }
                viewModel.ShiftReports = preShiftReportViewModels;
            }

            var deviceMould = (deviceMoulds.FindAll(g => g.DeviceId == deviceId)).AsEnumerable();
            var deviceMouldViewModels = _mapper.Map<IEnumerable<DeviceMould>, IEnumerable<DeviceMouldViewModel>>(deviceMould);
            viewModel.Moulds = deviceMouldViewModels;
        }

        deviceViewModels = deviceViewModels.OrderByDescending(r => r.DisplayPriority);

        return deviceViewModels;
    }
}
