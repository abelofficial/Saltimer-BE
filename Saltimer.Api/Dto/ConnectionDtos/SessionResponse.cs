using AutoMapper;
using Saltimer.Api.Models;

namespace Saltimer.Api.Dto
{
    public class SessionResponse
    {
        public int Id { get; set; }

        public string UniqueId { get; set; }

        public string DisplayName { get; set; }

        public int RoundTime { get; set; }

        public int BreakTime { get; set; }

        public virtual IEnumerable<UserResponseDto>? Users { get; set; }

    }

    public class MobTimerSessionToSessionResponseProfile : Profile
    {
        public MobTimerSessionToSessionResponseProfile()
        {
            CreateMap<MobTimerSession, SessionResponse>();
            // .ForMember(
            //     dest => dest.Members,
            //     opt => opt.MapFrom(src => src.Members.Select(m => m.User)));
        }
    }
}
