using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Queries
{
    public class GetUserByIdQuery : IRequest<UserResponseDto>
    {
        public int Id { get; set; }
    }
}
