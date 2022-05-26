using AutoMapper;
using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Commands;
public class GetCurrentUserHandler : BaseHandler, IRequestHandler<GetCurrentUserQuery, UserResponseDto>
{
    public GetCurrentUserHandler(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<UserResponseDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();
        var response = _mapper.Map<UserResponseDto>(currentUser);
        return await Task.FromResult(response);
    }
}