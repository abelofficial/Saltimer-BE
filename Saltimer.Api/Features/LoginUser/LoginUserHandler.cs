using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Handlers;
public class LoginUserHandler : BaseHandler, IRequestHandler<LoginUserCommand, LoginResponse>
{
    public LoginUserHandler(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<LoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var targetUser = await _context.User.SingleOrDefaultAsync(c => c.Username == request.Username);

        if (targetUser == null || !_authService.VerifyPasswordHash(request.Password, targetUser.PasswordHash, targetUser.PasswordSalt))
        {
            throw new HttpRequestException("Unauthorized", null, HttpStatusCode.Unauthorized);
        }

        string token = _authService.CreateToken(targetUser);
        return new LoginResponse() { Token = token };
    }
}