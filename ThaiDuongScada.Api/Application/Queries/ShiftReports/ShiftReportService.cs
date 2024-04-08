using AutoMapper;
using System.Runtime.CompilerServices;
using System.Text;

namespace ThaiDuongScada.Api.Application.Queries.ShiftReports;

public class ShiftReportService : IShiftReportService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IShiftReportRepository _shiftReportRepository;
    private readonly IMapper _mapper;

    public ShiftReportService(IDeviceRepository deviceRepository, IShiftReportRepository shiftReportRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _shiftReportRepository = shiftReportRepository;
        _mapper = mapper;
    }

    public async Task<List<IEnumerable<ShiftReportViewModel>>> GetAllSecondLatestShiftReports()
    {
        List<IEnumerable<ShiftReportViewModel>> allPreShiftReports = new();

        List<ShiftReport> iPreShiftReports = new();
        List<ShiftReport> rPreShiftReports = new();
        List<ShiftReport> lPreShiftReports = new();

        var shiftReports = await GetAllPreNewShiftReports();

        foreach (var shiftReport in shiftReports)
        {
            if (shiftReport.DeviceId.StartsWith("I"))
            {
                switch (shiftReport.MouldSlot)
                {
                    case 1:
                        shiftReport.DeviceId += "-1";
                        break;
                }

                iPreShiftReports.Add(shiftReport);
            }

            else if (shiftReport.DeviceId.StartsWith("P"))
            {
                switch (shiftReport.MouldSlot)
                {
                    case 1:
                        shiftReport.DeviceId += "-D";
                        break;
                    case 2:
                        shiftReport.DeviceId += "-U";
                        break;
                }
                rPreShiftReports.Add(shiftReport);
            }

            else if (shiftReport.DeviceId.StartsWith("L"))
            {
                switch (shiftReport.MouldSlot)
                {
                    case 1:
                        shiftReport.DeviceId += "-1";
                        break;
                    case 2:
                        shiftReport.DeviceId += "-2";
                        break;
                    case 3:
                        shiftReport.DeviceId += "-3";
                        break;
                    case 4:
                        shiftReport.DeviceId += "-4";
                        break;
                }
                lPreShiftReports.Add(shiftReport);
            }
        }

        iPreShiftReports.Sort();
        rPreShiftReports.Sort();
        lPreShiftReports.Sort();

        var iPreShiftReportsViewModel = _mapper.Map<IEnumerable<ShiftReport>, IEnumerable<ShiftReportViewModel>>(iPreShiftReports);
        var rPreShiftReportsViewModel = _mapper.Map<IEnumerable<ShiftReport>, IEnumerable<ShiftReportViewModel>>(rPreShiftReports);
        var lPreShiftReportsViewModel = _mapper.Map<IEnumerable<ShiftReport>, IEnumerable<ShiftReportViewModel>>(lPreShiftReports);

        allPreShiftReports.Add(iPreShiftReportsViewModel);
        allPreShiftReports.Add(rPreShiftReportsViewModel);
        allPreShiftReports.Add(lPreShiftReportsViewModel);

        return allPreShiftReports;
    }

    private async Task<IEnumerable<ShiftReport>> GetAllPreNewShiftReports()
    {
        var listShiftReports = new List<ShiftReport>();

        var devices = (await _deviceRepository.GetAllDevice()).Select(c => c.DeviceId);

        var shiftReports = await _shiftReportRepository.GetAllSecondLatestShiftReports();
        var listShiftReportId = shiftReports.Select(c => c.DeviceId);

        var deviceWithOutReports = devices.Except(listShiftReportId);
        ShiftReport shiftReport = shiftReports.First();

        foreach (var deviceWithOutReport in deviceWithOutReports)
        {
            if (deviceWithOutReport.StartsWith("P"))
            {
                listShiftReports.Add(new ShiftReport(shiftReport.ShiftNumber, shiftReport.Date, deviceWithOutReport, 1, 0, 0, new List<Shot>()));
                listShiftReports.Add(new ShiftReport(shiftReport.ShiftNumber, shiftReport.Date, deviceWithOutReport, 2, 0, 0, new List<Shot>()));
            }
            else if (deviceWithOutReport.StartsWith("L"))
            {
                listShiftReports.Add(new ShiftReport(shiftReport.ShiftNumber, shiftReport.Date, deviceWithOutReport, 1, 0, 0, new List<Shot>()));
                listShiftReports.Add(new ShiftReport(shiftReport.ShiftNumber, shiftReport.Date, deviceWithOutReport, 2, 0, 0, new List<Shot>()));
                listShiftReports.Add(new ShiftReport(shiftReport.ShiftNumber, shiftReport.Date, deviceWithOutReport, 3, 0, 0, new List<Shot>()));
                listShiftReports.Add(new ShiftReport(shiftReport.ShiftNumber, shiftReport.Date, deviceWithOutReport, 4, 0, 0, new List<Shot>()));
            }
            else if (deviceWithOutReport.StartsWith("I"))
            {
                listShiftReports.Add(new ShiftReport(shiftReport.ShiftNumber, shiftReport.Date, deviceWithOutReport, 1, 0, 0, new List<Shot>()));
            }
        }

        listShiftReports.AddRange(shiftReports);

        return listShiftReports;
    }
}
