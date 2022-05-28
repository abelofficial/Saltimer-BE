using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Models;

namespace Saltimer.Api.Handlers;
public class CreateSessionMemberHandler : BaseHandler, IRequestHandler<CreateSessionMemberCommand, SessionMemberResponse>
{


    public CreateSessionMemberHandler(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<SessionMemberResponse> Handle(CreateSessionMemberCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();
        var targetUser = await _context.User.FindAsync(request.UserId);

        if (targetUser == null) throw new HttpRequestException("Target user not found.", null, HttpStatusCode.NotFound);

        var targetMobTimer = await _context.SessionMember
                .Include(sm => sm.Session.Members)
                .Where(sm => sm.User.Id == currentUser.Id)
                .Where(sm => sm.Session.Id == request.MobTimerId)
                .Select(sm => sm.Session)
                .FirstOrDefaultAsync();

        if (targetMobTimer == null) throw new HttpRequestException("Mobtimer session not found.", null, HttpStatusCode.NotFound);


        var userAlreadyMember = _context.SessionMember
                .Any(sm => sm.Session.Id == request.MobTimerId && sm.User.Id == targetUser.Id);

        if (userAlreadyMember) throw new HttpRequestException("Provided user is already a member.", null, HttpStatusCode.NotFound);

        var newRecord = _context.SessionMember.Add(new SessionMember()
        {
            User = targetUser,
            Session = targetMobTimer,
            Turn = targetMobTimer.Members.Max(m => m.Turn) + 1,
        }).Entity;
        await _context.SaveChangesAsync();

        var sessionMember = _context.SessionMember
                    .Include(sm => sm.User)
                    .Include(sm => sm.Session)
                    .Where(s => s.Id == newRecord.Id)
                    .Single();

        var response = _mapper.Map<SessionMemberResponse>(sessionMember);

        return response;
    }
}