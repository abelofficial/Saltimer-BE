using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Handlers;
public class RemoveSessionMemberHandler : BaseHandler, IRequestHandler<RemoveSessionMemberCommand>
{


    public RemoveSessionMemberHandler(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<Unit> Handle(RemoveSessionMemberCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();

        var sessionMember = await _context.SessionMember
                    .Include(sm => sm.Session.Owner)
                    .Include(sm => sm.User)
                    .SingleOrDefaultAsync(sm => sm.Session.Id == request.MobTimerId && sm.User.Id == request.UserId);

        if (sessionMember == null) throw new HttpRequestException(HttpStatusCode.GetName(HttpStatusCode.NotFound), null, HttpStatusCode.NotFound);


        if (sessionMember.Session.Owner.Id != currentUser.Id && sessionMember.User.Id != currentUser.Id)
            throw new HttpRequestException(HttpStatusCode.GetName(HttpStatusCode.Forbidden), null, HttpStatusCode.Forbidden);

        if (sessionMember.Session.Owner.Id == currentUser.Id && sessionMember.User.Id == currentUser.Id)
        {
            var mobTimer = await _context.MobTimerSession
                    .Include(ms => ms.Members)
                    .SingleOrDefaultAsync(ms => ms.Id == request.MobTimerId);

            _context.RemoveRange(mobTimer);
            _context.RemoveRange(mobTimer.Members);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }

        _context.SessionMember.Remove(sessionMember);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}