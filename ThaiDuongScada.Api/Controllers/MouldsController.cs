using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using ThaiDuongScada.Api.Application.Queries;
using ThaiDuongScada.Api.Application.Queries.Moulds;

namespace ThaiDuongScada.Api.Controllers;
[Route("api/[controller]")]
[ApiController]

public class MouldsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MouldsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<MouldViewModel>> GetMoulds([FromQuery] MouldsQuery query)
    {
        return await _mediator.Send(query);
    }
}
