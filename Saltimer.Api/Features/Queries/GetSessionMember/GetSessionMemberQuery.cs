using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Queries
{
    public class GetSessionMemberQuery : IRequest<IEnumerable<UserResponseDto>>
    {
        public int SessionId { get; set; }
    }
}
