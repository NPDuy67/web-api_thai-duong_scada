using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using ThaiDuongScada.Api.Application.Queries;
using ThaiDuongScada.Api.Application.Queries.ShiftReports;

namespace ThaiDuongScada.Api.Controllers;
[Route("api/[controller]")]
[ApiController]

public class ShiftReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IShiftReportService _shiftReportService;

    public ShiftReportsController(IMediator mediator, IShiftReportService shiftReportService)
    {
        _mediator = mediator;
        _shiftReportService = shiftReportService;
    }

    [HttpGet]
    public async Task<IEnumerable<ShiftReportViewModel>> GetListAsync([FromQuery] ShiftReportsQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpGet]
    [Route("secondLatest")]
    public async Task<List<IEnumerable<ShiftReportViewModel>>> GetAllSecondLatestShiftReports()
    {
        return await _shiftReportService.GetAllSecondLatestShiftReports();
    }

    [HttpGet]
    [Route("downloadReport")]
    public async Task<IActionResult> DownLoadExcelReport([FromQuery] DownloadReportsQuery query)
    {
        var file = await _mediator.Send(query);
        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OEEreport.xlsx");
    }

    [HttpGet]
    [Route("shiftId")]
    public async Task<IEnumerable<ShiftReportWithShotViewModel>> GetByShiftReportId([FromQuery] ShiftReportWithShotsQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpGet]
    [Route("average")]
    public async Task<ShiftReportAverageViewModel> GetAverageOEE([FromQuery] ShiftReportAverageQuery query)
    {
        return await _mediator.Send(query);
    }
}
