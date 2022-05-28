using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserResponseDto>>
    {
        public string? Filter { get; set; }
    }
}
