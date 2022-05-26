using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Queries
{
    public class GetSessionsQuery : IRequest<IEnumerable<MobTimerResponse>>
    {

    }
}
