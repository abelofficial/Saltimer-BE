using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Queries
{
    public class GetSessionByIdQuery : IRequest<MobTimerResponse>
    {
        public int Id { get; set; }
    }
}
