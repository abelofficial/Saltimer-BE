using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Handlers;
public class GetSessionMemberHandler : BaseHandler, IRequestHandler<GetSessionMemberQuery, IEnumerable<UserResponseDto>>
{
    public GetSessionMemberHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<IEnumerable<UserResponseDto>> Handle(GetSessionMemberQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();
        var targetMobTimer = await _context.SessionMember
                .Where(sm => sm.User.Id == currentUser.Id)
                .Where(sm => sm.Session.Id == request.SessionId)
                .Select(sm => sm.Session)
                .FirstOrDefaultAsync();

        if (targetMobTimer == null)
            throw new HttpRequestException("Mob timer session not found.", null, HttpStatusCode.NotFound);

        return await _context.SessionMember
                .Where(sm => sm.Session.Id == targetMobTimer.Id)
                .Select(sm => _mapper.Map<UserResponseDto>(sm.User))
                .ToListAsync();
    }
}