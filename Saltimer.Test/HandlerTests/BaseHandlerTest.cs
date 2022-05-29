using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Saltimer.Api.Mapping;
using Saltimer.Api.Services;
using Saltimer.Test.Mocks;

namespace Saltimer.Test.HandlerTests;

public abstract class BaseHandlerTest
{

    public readonly IMapper _mapper;
    public Mock<IAuthService> _mockAuthService;

    protected BaseHandlerTest(DbContextOptions<SaltimerDBContext> context)
    {
        ContextOptions = context;
        Seed();

        _mockAuthService = MockAuthService.GetAuthService();

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

            var adminUser = context.Add(MockUsers.GetAdminUser()).Entity;
            var uniqueUser = context.Add(MockUsers.GetUniqueUser(1)).Entity;

            var mobSessionData = context.Add(MockMobTimers.GetMobTimer(adminUser)).Entity;

            adminUser.MobTimers.Add(mobSessionData);
            mobSessionData.Owner = adminUser;
            mobSessionData.Members.Add(MockMobTimers.GetMobTimerMember(adminUser, 1, mobSessionData));
            mobSessionData.Members.Add(MockMobTimers.GetMobTimerMember(uniqueUser, 2, mobSessionData));

            context.SaveChanges();

        }
    }



}



