using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using ThaiDuongScada.Api.Application.Commands.DeviceMoulds;
using ThaiDuongScada.Api.Application.Commands.DeviceMouldsl;
using ThaiDuongScada.Api.Application.Queries;
using ThaiDuongScada.Api.Application.Queries.DeviceMoulds;

namespace ThaiDuongScada.Api.Controllers;
[Route("api/[controller]")]
[ApiController]

public class DeviceMouldsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeviceMouldsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDeviceMould(string deviceId, int mouldSlot, [FromBody] UpdateDeviceMouldViewModel deviceMould)
    {
        var command = new UpdateDeviceMouldCommand(deviceId, mouldSlot, deviceMould.MouldId, deviceMould.PreviousShotCount);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateShiftTimeAsync(string deviceType, [FromBody] UpdateShiftTimeViewModel deviceMould)
    {
        var command = new UpdateShiftTimeCommand(deviceType, deviceMould.ShiftDuration);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    [Route("shiftTime")]
    public async Task<EShiftDuration> GetDeviceTypeShiftTimeAsync([FromQuery] DeviceMouldsQuery query)
    {
        return await _mediator.Send(query);
    }
} 
