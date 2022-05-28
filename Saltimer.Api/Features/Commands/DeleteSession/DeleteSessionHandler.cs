using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Handlers;
public class DeleteSessionHandler : BaseHandler, IRequestHandler<DeleteSessionCommand>
{


    public DeleteSessionHandler(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<Unit> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();
        var mobTimerSession = await _context.MobTimerSession
            .Include(ms => ms.Members)
            .Where(ms => ms.Owner.Id == currentUser.Id)
            .Where(ms => ms.Id == request.Id)
            .SingleOrDefaultAsync();

        if (mobTimerSession == null)
        {
            throw new HttpRequestException(HttpStatusCode.GetName(HttpStatusCode.NotFound), null, HttpStatusCode.NotFound);
        }

        _context.RemoveRange(mobTimerSession);
        _context.RemoveRange(mobTimerSession.Members);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}