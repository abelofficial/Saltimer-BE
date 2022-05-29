using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Saltimer.Api.Command
{
    public class RemoveSessionMemberCommand : IRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int MobTimerId { get; set; }
    }


}
