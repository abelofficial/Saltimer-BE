using AutoMapper;
using Saltimer.Api.Command;
using Saltimer.Api.Dto;
using Saltimer.Api.Models;

namespace Saltimer.Api.Mapping;
public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<CreateSessionCommand, MobTimerSession>();
        CreateMap<RegisterUserCommand, User>();
        CreateMap<UpdateUserCommand, User>();
        CreateMap<MobTimerSession, CreateSessionCommand>();
        CreateMap<MobTimerSession, MobTimerResponse>();
        CreateMap<User, UserResponseDto>();
    }


}