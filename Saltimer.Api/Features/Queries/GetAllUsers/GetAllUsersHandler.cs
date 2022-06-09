using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;
using Saltimer.Api.Services;

namespace Saltimer.Api.Handlers;
public class GetAllUsersHandler : BaseHandler, IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponseDto>>
{
    public GetAllUsersHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context) { }

    public async Task<IEnumerable<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.User
              .Where(u => request.Filter == null ? true :
                          u.Username.Contains(request.Filter) ||
                          u.EmailAddress.Contains(request.Filter))
              .Select(u => _mapper.Map<UserResponseDto>(u))
              .ToListAsync();
    }
}