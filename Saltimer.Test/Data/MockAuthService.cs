using Moq;
using Saltimer.Api.Services;

namespace Saltimer.Test.Mocks;

public static class MockAuthService
{

    public static Mock<IAuthService> GetAuthService()
    {
        var mockAuthService = new Mock<IAuthService>();


        mockAuthService.Setup(s => s.GetCurrentUser()).Returns(MockUsers.GetAdminUser());

        return mockAuthService;
    }
}



