using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Queries
{
    public class GetSessionByUniqueIdQuery : IRequest<MobTimerResponse>
    {
        public Guid UniqueId { get; set; }
    }
}
