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

            var data = new List<User>() {
                MockUsers.GetAdminUser(),
                MockUsers.GetUniqueUser(1)
            };

            context.AddRange(data);
            context.SaveChanges();
        }
    }


}



