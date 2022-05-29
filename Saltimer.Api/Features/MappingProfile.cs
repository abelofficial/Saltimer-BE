using AutoMapper;
using Saltimer.Api.Command;
using Saltimer.Api.Models;

namespace Saltimer.Api.Handlers;
public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<CreateSessionCommand, MobTimerSession>();
        CreateMap<RegisterUserCommand, User>();
        CreateMap<UpdateUserCommand, User>();
    }


}