using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;
using Saltimer.Api.Services;

namespace Saltimer.Api.Handlers;
public class GetSessionsHandler : BaseHandler, IRequestHandler<GetSessionsQuery, IEnumerable<MobTimerResponse>>
{
    public GetSessionsHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<IEnumerable<MobTimerResponse>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();

        return await _context.MobTimerSession
            .Where(m => m.Owner.Username.Equals(currentUser.Username) ||
                        m.Members.Any(s => s.User.Username.Equals(currentUser.Username)))
            .Include(e => e.Members)
            .Select(m => _mapper.Map<MobTimerResponse>(m))
            .ToListAsync();
    }
}