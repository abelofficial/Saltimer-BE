using Moq;
using Saltimer.Api.Models;
using Saltimer.Api.Services;

namespace Saltimer.Test.Mocks;

public static class MockAuthService
{

    public static Mock<IAuthService> GetAuthService()
    {
        var mockAuthService = new Mock<IAuthService>();


        mockAuthService.Setup(s => s.GetCurrentUser()).Returns(new User()
        {
            Id = 100,

        });

        return mockAuthService;
    }


}



