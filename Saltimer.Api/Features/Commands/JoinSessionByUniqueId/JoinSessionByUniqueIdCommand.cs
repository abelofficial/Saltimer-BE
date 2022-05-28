using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Saltimer.Api.Dto
{
    public class JoinSessionByUniqueIdCommand : IRequest<SessionMemberResponse>
    {
        [Required]
        public Guid Uuid { get; set; }
    }


}
