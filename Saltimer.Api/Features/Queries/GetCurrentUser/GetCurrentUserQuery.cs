using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Queries
{
    public class GetCurrentUserQuery : IRequest<UserResponseDto>
    {

    }
}
