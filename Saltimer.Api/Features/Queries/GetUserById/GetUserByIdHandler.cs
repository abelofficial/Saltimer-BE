using System.Net;
using AutoMapper;
using MediatR;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Handlers;
public class GetUserByIdHandler : BaseHandler, IRequestHandler<GetUserByIdQuery, UserResponseDto>
{
    public GetUserByIdHandler(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.User.FindAsync(request.Id);

        if (user == null)
        {
            throw new HttpRequestException(HttpStatusCode.GetName(HttpStatusCode.NotFound), null, HttpStatusCode.NotFound);
        }

        return _mapper.Map<UserResponseDto>(user);
    }
}