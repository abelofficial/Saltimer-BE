using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Handlers;
public class GetSessionByIdHandler : BaseHandler, IRequestHandler<GetSessionByIdQuery, MobTimerResponse>
{
    public GetSessionByIdHandler(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<MobTimerResponse> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();

        var mobTimerSession = await _context.MobTimerSession.Where(m => m.Owner.Username.Equals(currentUser.Username) ||
                        m.Members.Any(s => s.User.Username.Equals(currentUser.Username)))
            .Include(e => e.Members)
            .Where(m => m.Id == request.Id)
            .Select(m => _mapper.Map<MobTimerResponse>(m)).SingleOrDefaultAsync();

        if (mobTimerSession == null)
        {
            throw new HttpRequestException("Not found", null, HttpStatusCode.NotFound);
        }

        return mobTimerSession;
    }
}