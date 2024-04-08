using Microsoft.AspNetCore.Mvc;
using ThaiDuongScada.Api.Application.Queries;
using ThaiDuongScada.Api.Application.Queries.Devices;

namespace ThaiDuongScada.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DevicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<DeviceViewModel>> GetDevices([FromQuery] DevicesQuery query)
    {
        return await _mediator.Send(query);
    }
}
    


