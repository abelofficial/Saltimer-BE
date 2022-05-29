using System.Threading;
using efCdCollection.Tests.Data;
using FluentAssertions;
using Saltimer.Api.Dto;
using Saltimer.Api.Handlers;
using Saltimer.Api.Queries;
using Saltimer.Test.Mocks;
using Xunit;

namespace Saltimer.Test.HandlerTests;

public class GetCurrentUserTest : SqliteInMemory
{
    private readonly SaltimerDBContext _context;
    private readonly GetCurrentUserHandler _handler;

    public GetCurrentUserTest() : base()
    {
        _context = new SaltimerDBContext(ContextOptions);
        _handler = new GetCurrentUserHandler(_mapper, _mockAuthService.Object, _context);
    }

    [Fact]
    public async void Should_be_abel_to_fetch_all_users()
    {
        var query = new GetCurrentUserQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeOfType<UserResponseDto>();

        result.Username.Should().Be(MockUsers.GetAdminUser().Username);
    }


}



