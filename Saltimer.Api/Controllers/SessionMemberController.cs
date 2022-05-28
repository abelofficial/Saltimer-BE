#nullable disable
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Controllers
{
    public class SessionMemberController : BaseController
    {
        private IMediator _mediator;
        public SessionMemberController(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context)
        {
            _mediator = mediator;
        }

        // GET: api/SessionMember
        [HttpGet("{mobTimerId}")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetSessionMember(int mobTimerId)
        {
            return Ok(await _mediator.Send(new GetSessionMemberQuery() { SessionId = mobTimerId }));
        }

        // GET: api/SessionMember
        [HttpGet("vip/{uuid}")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetSessionMemberByUUID(Guid uuid)
        {
            return Ok(await _mediator.Send(new GetMemberByUniqueIdQuery() { UniqueId = uuid }));
        }

        [HttpPost("{mobTimerId}")]
        [ActionName(nameof(PostSessionMember))]
        public async Task<ActionResult<SessionMemberResponse>> PostSessionMember(int mobTimerId, SessionMemberRequest request)
        {

            var response = await _mediator.Send(new CreateSessionMemberCommand()
            {
                MobTimerId = mobTimerId,
                UserId = request.UserId
            });

            return CreatedAtAction(nameof(PostSessionMember), response);
        }

        [HttpPost("vip")]
        [ActionName(nameof(PostSessionMember))]
        public async Task<ActionResult<SessionMemberResponse>> PostSessionMemberByUUID(JoinSessionByUniqueIdCommand request)
        {
            var response = await _mediator.Send(request);

            return CreatedAtAction(nameof(PostSessionMember), response);
        }


        // DELETE: api/SessionMember/5
        [HttpDelete("{mobTimerId}")]
        public async Task<IActionResult> DeleteSessionMember(int mobTimerId, SessionMemberRequest request)
        {
            await _mediator.Send(new RemoveSessionMemberCommand()
            {
                MobTimerId = mobTimerId,
                UserId = request.UserId
            });

            return NoContent();
        }

        private bool SessionMemberExists(int id)
        {
            return _context.SessionMember.Any(e => e.Id == id);
        }
    }
}
