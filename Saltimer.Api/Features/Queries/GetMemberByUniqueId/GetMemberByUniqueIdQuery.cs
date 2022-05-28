using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Queries
{
    public class GetMemberByUniqueIdQuery : IRequest<IEnumerable<UserResponseDto>>
    {
        public Guid UniqueId { get; set; }
    }
}
