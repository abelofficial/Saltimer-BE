using AutoMapper;
using MediatR;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Handlers;
public class GetCurrentUserHandler : BaseHandler, IRequestHandler<GetCurrentUserQuery, UserResponseDto>
{
    public GetCurrentUserHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<UserResponseDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _authService.GetCurrentUser();
        var response = _mapper.Map<UserResponseDto>(currentUser);
        return await Task.FromResult(response);
    }
}