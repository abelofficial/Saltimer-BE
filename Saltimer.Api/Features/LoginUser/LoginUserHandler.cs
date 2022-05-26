using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Commands;
public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginResponse>
{

    protected readonly SaltimerDBContext _context;
    protected readonly IAuthService _authService;
    protected readonly IMapper _mapper;
    public LoginUserHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
    {
        _mapper = mapper;
        _authService = authService;
        _context = context;
    }
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