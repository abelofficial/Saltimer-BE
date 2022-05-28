using AutoMapper;
using MediatR;
using Saltimer.Api.Dto;
using Saltimer.Api.Models;

namespace Saltimer.Api.Handlers;
public class CreateSessionHandler : BaseHandler, IRequestHandler<CreateSessionCommand, MobTimerResponse>
{


    public CreateSessionHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<MobTimerResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
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

        return response;
    }
}