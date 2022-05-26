#nullable disable
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Models;
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
        public async Task<ActionResult<MobTimerResponse>> PostMobTimerSession(CreateMobTimerDto request)
        {
            var currentUser = _authService.GetCurrentUser();
            var newMobTimer = _mapper.Map<MobTimerSession>(request);
            newMobTimer.Owner = currentUser;
            newMobTimer = _context.MobTimerSession.Add(newMobTimer).Entity;
            _context.SessionMember.Add(new SessionMember()
            {
                User = currentUser,
                Session = newMobTimer
            });

            await _context.SaveChangesAsync();
            var response = _mapper.Map<MobTimerResponse>(newMobTimer);

            return CreatedAtAction("GetMobTimerSession", new { id = response.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMobTimerSession(int id)
        {
            var currentUser = _authService.GetCurrentUser();
            var mobTimerSession = await _context.MobTimerSession
                .Include(ms => ms.Members)
                .Where(ms => ms.Owner.Id == currentUser.Id)
                .Where(ms => ms.Id == id)
                .SingleOrDefaultAsync();

            if (mobTimerSession == null)
            {
                return NotFound();
            }

            _context.RemoveRange(mobTimerSession);
            _context.RemoveRange(mobTimerSession.Members);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool mobTimerSessionExists(int id)
        {
            return _context.MobTimerSession.Any(e => e.Id == id);
        }
    }
}
