using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Commands;
public class UpdateUserHandler : BaseHandler, IRequestHandler<UpdateUserCommand>
{


    public UpdateUserHandler(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();
        var targetUser = await _context.User.Where(u => u.Id == currentUser.Id).SingleOrDefaultAsync();

        if (_context.User.Any(e => e.Id != targetUser.Id && e.Username == request.Username))
            throw new HttpRequestException("Username is already taken.", null, HttpStatusCode.BadRequest);

        if (_context.User.Any(e => e.Id != targetUser.Id && e.EmailAddress == request.EmailAddress))
            throw new HttpRequestException("Email address is already taken.", null, HttpStatusCode.BadRequest);

        _mapper.Map(request, targetUser);
        _context.Entry(targetUser).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}