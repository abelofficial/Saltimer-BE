#nullable disable
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Saltimer.Api.Attributes;
using Saltimer.Api.Command;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ValidateTokenAttribute]
    [ApiController]
    public class MobTimerController : ControllerBase
    {
        private IMediator _mediator;
        public MobTimerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all mob timer session for the current user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MobTimerResponse>>> GetMobTimerSession()
        {
            return Ok(await _mediator.Send(new GetSessionsQuery()));
        }

        /// <summary>
        /// Get a mob timer session by id for the current user.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MobTimerResponse>> GetMobTimerSession(int id)
        {
            return Ok(await _mediator.Send(new GetSessionByIdQuery() { Id = id }));
        }

        /// <summary>
        /// Get a mob timer session by unique id for the current user.
        /// </summary>
        [HttpGet("vip/{uuid}")]
        public async Task<ActionResult<MobTimerResponse>> GetMobTimerSessionByUUID(Guid uuid)
        {
            return Ok(await _mediator.Send(new GetSessionByUniqueIdQuery() { UniqueId = uuid }));
        }

        /// <summary>
        /// Create a new a mob timer session for the current user.
        /// </summary>
        [SwaggerResponse((int)HttpStatusCode.NoContent, typeof(MobTimerResponse), Description = "Newly created mob timer session")]
        [HttpPost]
        public async Task<ActionResult<MobTimerResponse>> PostMobTimerSession(CreateSessionCommand request)
        {

            var response = await _mediator.Send(request);

            return CreatedAtAction("GetMobTimerSession", new { id = response.Id }, response);
        }

        /// <summary>
        /// Delete a mob timer session from the current user.
        /// </summary>
        [SwaggerResponse((int)HttpStatusCode.NoContent, null, Description = "successfully removed a session.")]

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
