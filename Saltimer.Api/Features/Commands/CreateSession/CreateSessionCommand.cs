using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Saltimer.Api.Models;

namespace Saltimer.Api.Dto
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

    public class CreateSessionCommandToMobTimerSessionProfile : Profile
    {
        public CreateSessionCommandToMobTimerSessionProfile()
        {
            CreateMap<CreateSessionCommand, MobTimerSession>();
        }
    }
}
