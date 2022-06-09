using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using efCdCollection.Tests.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Command;
using Saltimer.Api.Handlers;
using Saltimer.Test.Mocks;
using Xunit;

namespace Saltimer.Test.HandlerTests;

public class CreateSessionMemberMemberTest : SqliteInMemory
{
    private readonly SaltimerDBContext _context;
    private readonly CreateSessionMemberHandler _handler;

    public CreateSessionMemberMemberTest() : base()
    {
        _context = new SaltimerDBContext(ContextOptions);
        _handler = new CreateSessionMemberHandler(_mapper, _mockAuthService.Object, _context);
    }

    [Fact]
    public async void Should_throw_not_found_exception_if_user_is_not_found()
    {
        var _command = new CreateSessionMemberCommand()
        {
            UserId = 100,
            MobTimerId = 100,
        };

        var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _handler.Handle(_command, CancellationToken.None));
        ((HttpRequestException)exception).StatusCode.Should().Be(HttpStatusCode.NotFound);
        ((HttpRequestException)exception).Message.Should().Be("Target user not found.");
    }

    [Fact]
    public async void Should_throw_not_found_exception_if_mobSession_is_not_found()
    {
        var _command = new CreateSessionMemberCommand()
        {
            UserId = _context.User.First().Id,
            MobTimerId = 100,
        };

        var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _handler.Handle(_command, CancellationToken.None));
        ((HttpRequestException)exception).StatusCode.Should().Be(HttpStatusCode.NotFound);
        ((HttpRequestException)exception).Message.Should().Be("Mobtimer session not found.");
    }

    [Fact]
    public async void Should_throw_bad_request_exception_if_user_is_already_member()
    {
        var uniqueUser = await _context.User.SingleAsync(u => u.Username.Equals(MockUsers.GetUniqueUser(1).Username));

        var session = await _context.MobTimerSession.FirstAsync();

        var _command = new CreateSessionMemberCommand()
        {
            UserId = uniqueUser.Id,
            MobTimerId = session.Id,
        };

        var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _handler.Handle(_command, CancellationToken.None));
        ((HttpRequestException)exception).Message.Should().Be("Provided user is already a member.");
        ((HttpRequestException)exception).StatusCode.Should().Be(HttpStatusCode.BadRequest);

    }


}



