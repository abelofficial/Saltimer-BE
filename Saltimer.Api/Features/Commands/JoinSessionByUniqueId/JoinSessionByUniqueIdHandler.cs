using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Command;
using Saltimer.Api.Dto;
using Saltimer.Api.Models;
using Saltimer.Api.Services;

namespace Saltimer.Api.Handlers;
public class JoinSessionByUniqueIdHandler : BaseHandler, IRequestHandler<JoinSessionByUniqueIdCommand, SessionMemberResponse>
{


    public JoinSessionByUniqueIdHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<SessionMemberResponse> Handle(JoinSessionByUniqueIdCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();

        var targetMobTimer = await _context.SessionMember
                .Include(sm => sm.Session.Members)
                .Where(sm => sm.Session.UniqueId.Equals(request.Uuid.ToString()))
                .Select(sm => sm.Session)
                .FirstOrDefaultAsync();

        if (targetMobTimer == null) throw new HttpRequestException("Mobtimer session not found.", null, HttpStatusCode.NotFound);

        var userAlreadyMember = _context.SessionMember
                .Any(sm => request.Uuid.ToString().Equals(sm.Session.UniqueId) && sm.User.Id == currentUser.Id);

        if (userAlreadyMember) throw new HttpRequestException("Provided user is already a member.", null, HttpStatusCode.NotFound);

        var newRecord = _context.SessionMember.Add(new SessionMember()
        {
            User = currentUser,
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