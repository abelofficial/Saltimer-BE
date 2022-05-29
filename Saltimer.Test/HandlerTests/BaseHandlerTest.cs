using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Saltimer.Api.Mapping;
using Saltimer.Api.Models;
using Saltimer.Api.Services;
using Saltimer.Test.Mocks;

namespace Saltimer.Test.HandlerTests;

public abstract class BaseHandlerTest
{

    public readonly IMapper _mapper;
    public readonly Mock<IAuthService> _mockAuthService;

    protected BaseHandlerTest(DbContextOptions<SaltimerDBContext> context)
    {
        _mockAuthService = MockAuthService.GetAuthService();
        ContextOptions = context;
        Seed();

        var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });


        _mapper = mapperConfig.CreateMapper();
    }


    protected DbContextOptions<SaltimerDBContext> ContextOptions { get; }

    private void Seed()
    {
        using (var context = new SaltimerDBContext(ContextOptions))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var data = new List<User>() {
                new User() {
                    Username =  "",
                    FirstName =  "",
                    LastName =  "",
                    EmailAddress =  "",
                    PasswordHash =  new byte[0],
                    PasswordSalt =  new byte[0],
                    MobTimers =  new List<MobTimerSession>(),

                }
            };

            context.AddRange(data); context.SaveChanges();
        }
    }


}



