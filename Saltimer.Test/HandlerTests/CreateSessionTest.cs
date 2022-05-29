using System.Threading;
using efCdCollection.Tests.Data;
using FluentAssertions;
using Saltimer.Api.Command;
using Saltimer.Api.Dto;
using Saltimer.Api.Handlers;
using Xunit;

namespace Saltimer.Test.HandlerTests;

public class CreateSessionTest : SqliteInMemory
{
    private readonly SaltimerDBContext _context;
    private readonly CreateSessionHandler _handler;

    public CreateSessionTest() : base()
    {
        _context = new SaltimerDBContext(ContextOptions);
        _handler = new CreateSessionHandler(_mapper, _mockAuthService.Object, _context);
    }

    [Fact]
    public async void Should_be_abel_to_create_new_session()
    {
        var _command = new CreateSessionCommand()
        {
            DisplayName = "Test",
            RoundTime = 30,
            BreakTime = 30,
        };

        var result = await _handler.Handle(_command, CancellationToken.None);

        result.Should().BeOfType<MobTimerResponse>();
    }


}



