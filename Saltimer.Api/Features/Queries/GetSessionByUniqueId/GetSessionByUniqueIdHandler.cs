using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Handlers;
public class GetSessionByUniqueIdHandler : BaseHandler, IRequestHandler<GetSessionByUniqueIdQuery, MobTimerResponse>
{
    public GetSessionByUniqueIdHandler(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<MobTimerResponse> Handle(GetSessionByUniqueIdQuery request, CancellationToken cancellationToken)
    {
        var mobTimerSession = await _context.MobTimerSession
               .Where(m => m.UniqueId.Equals(request.UniqueId.ToString()))
               .Include(e => e.Members)
               .Where(m => m.UniqueId == request.UniqueId.ToString())
               .Select(m => _mapper.Map<MobTimerResponse>(m)).SingleOrDefaultAsync();

        if (mobTimerSession == null)
        {
            throw new HttpRequestException($"Mobtimer session {request.UniqueId} not found.", null, HttpStatusCode.NotFound);
        }

        return mobTimerSession;
    }
}