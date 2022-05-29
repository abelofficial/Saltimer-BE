using System.ComponentModel.DataAnnotations;
using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Command
{
    public class JoinSessionByUniqueIdCommand : IRequest<SessionMemberResponse>
    {
        [Required]
        public Guid Uuid { get; set; }
    }


}
