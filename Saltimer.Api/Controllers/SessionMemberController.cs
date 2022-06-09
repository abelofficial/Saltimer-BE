#nullable disable
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saltimer.Api.Attributes;
using Saltimer.Api.Command;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ValidateTokenAttribute]
    [ApiController]
    public class SessionMemberController : ControllerBase
    {
        private IMediator _mediator;
        public SessionMemberController(IMediator mediator)

        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all members for a given mob timer session.
        /// </summary>
        [HttpGet("{mobTimerId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponseDto>))]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetSessionMember(int mobTimerId)
        {
            return Ok(await _mediator.Send(new GetSessionMemberQuery() { SessionId = mobTimerId }));
        }

        /// <summary>
        /// Get all members for a given mob timer session unique id.
        /// </summary>
        [HttpGet("vip/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponseDto>))]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetSessionMemberByUUID(Guid uuid)
        {
            return Ok(await _mediator.Send(new GetMemberByUniqueIdQuery() { UniqueId = uuid }));
        }


        /// <summary>
        /// Add new member to a mob timer session.
        /// </summary>
        [HttpPost("{mobTimerId}")]
        [ActionName(nameof(PostSessionMember))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SessionMemberResponse))]
        public async Task<ActionResult<SessionMemberResponse>> PostSessionMember(int mobTimerId, SessionMemberRequest request)
        {

            var response = await _mediator.Send(new CreateSessionMemberCommand()
            {
                MobTimerId = mobTimerId,
                UserId = request.UserId
            });

            return CreatedAtAction(nameof(PostSessionMember), response);
        }

        /// <summary>
        /// Join a mob timer session for current user.
        /// </summary>
        [HttpPost("vip")]
        [ActionName(nameof(PostSessionMember))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SessionMemberResponse))]
        public async Task<ActionResult<SessionMemberResponse>> PostSessionMemberByUUID(JoinSessionByUniqueIdCommand request)
        {
            var response = await _mediator.Send(request);

            return CreatedAtAction(nameof(PostSessionMember), response);
        }


        /// <summary>
        /// Remove a member from a mob timer session.
        /// </summary>
        [HttpDelete("{mobTimerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteSessionMember(int mobTimerId, SessionMemberRequest request)
        {
            await _mediator.Send(new RemoveSessionMemberCommand()
            {
                MobTimerId = mobTimerId,
                UserId = request.UserId
            });

            return NoContent();
        }

    }
}
