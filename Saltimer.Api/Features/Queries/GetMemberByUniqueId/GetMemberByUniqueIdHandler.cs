using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;
using Saltimer.Api.Services;

namespace Saltimer.Api.Handlers;
public class GetMemberByUniqueIdHandler : BaseHandler, IRequestHandler<GetMemberByUniqueIdQuery, IEnumerable<UserResponseDto>>
{
    public GetMemberByUniqueIdHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<IEnumerable<UserResponseDto>> Handle(GetMemberByUniqueIdQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();
        var targetMobTimer = await _context.SessionMember
                .Where(sm => sm.Session.UniqueId.Equals(request.UniqueId.ToString()))
                .Select(sm => sm.Session)
                .FirstOrDefaultAsync();

        if (targetMobTimer == null) throw new HttpRequestException("mob timer session not found.", null, HttpStatusCode.NotFound);

        return await _context.SessionMember
                .Where(sm => sm.Session.Id == targetMobTimer.Id)
                .Select(sm => _mapper.Map<UserResponseDto>(sm.User))
                .ToListAsync();
    }
}