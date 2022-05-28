using System.Net;
using AutoMapper;
using MediatR;
using Saltimer.Api.Command;
using Saltimer.Api.Dto;
using Saltimer.Api.Models;

namespace Saltimer.Api.Handlers;
public class RegisterUserHandler : BaseHandler, IRequestHandler<RegisterUserCommand, UserResponseDto>
{

    public RegisterUserHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }
    public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (_context.User.Any(e => e.Username == request.Username))
            throw new HttpRequestException("Username is already taken.", null, HttpStatusCode.BadRequest);


        if (_context.User.Any(e => e.EmailAddress == request.EmailAddress))
            throw new HttpRequestException("Email address is already taken.", null, HttpStatusCode.BadRequest);


        _authService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var newUser = _mapper.Map<User>(request);
        newUser.PasswordHash = passwordHash;
        newUser.PasswordSalt = passwordSalt;

        newUser = _context.User.Add(newUser).Entity;
        await _context.SaveChangesAsync();

        var response = _mapper.Map<UserResponseDto>(newUser);

        return response;
    }
}