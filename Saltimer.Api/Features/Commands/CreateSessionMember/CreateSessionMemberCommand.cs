using System.ComponentModel.DataAnnotations;
using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Command
{
    public class CreateSessionMemberCommand : IRequest<SessionMemberResponse>
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int MobTimerId { get; set; }
    }


}
