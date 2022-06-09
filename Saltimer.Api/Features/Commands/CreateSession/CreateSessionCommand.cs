using System.ComponentModel.DataAnnotations;
using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Command
{
    public class CreateSessionCommand : IRequest<MobTimerResponse>
    {
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string DisplayName { get; set; }

        [Required]
        [Range(1, 30)]
        public int RoundTime { get; set; }

        [Required]
        [Range(1, 30)]
        public int BreakTime { get; set; }
    }
}
