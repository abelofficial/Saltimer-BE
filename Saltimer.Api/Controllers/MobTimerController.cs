#nullable disable
using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Controllers
{

    public class MobTimerController : BaseController
    {
        private IMediator _mediator;
        public MobTimerController(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
           : base(mapper, authService, context)
        {
            _mediator = mediator;
        }

        // GET: api/MobTimerSession
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MobTimerResponse>>> GetMobTimerSession()
        {
            return Ok(await _mediator.Send(new GetSessionsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MobTimerResponse>> GetMobTimerSession(int id)
        {
            return Ok(await _mediator.Send(new GetSessionByIdQuery() { Id = id }));
        }

        [HttpGet("vip/{uuid}")]
        public async Task<ActionResult<MobTimerResponse>> GetMobTimerSessionByUUID(Guid uuid)
        {
            return Ok(await _mediator.Send(new GetSessionByUniqueIdQuery() { UniqueId = uuid }));
        }

        [HttpPost]
        public async Task<ActionResult<MobTimerResponse>> PostMobTimerSession(CreateSessionCommand request)
        {

            var response = await _mediator.Send(request);

            return CreatedAtAction("GetMobTimerSession", new { id = response.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMobTimerSession(int id)
        {
            try
            {
                await _mediator.Send(new DeleteSessionCommand() { Id = id });
                return NoContent();
            }
            catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return NoContent();
            }

        }

    }
}
