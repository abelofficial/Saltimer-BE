using System.Collections.Generic;
using System.Threading;
using efCdCollection.Tests.Data;
using FluentAssertions;
using Saltimer.Api.Dto;
using Saltimer.Api.Handlers;
using Saltimer.Api.Queries;
using Xunit;

namespace Saltimer.Test.HandlerTests;

public class GetAllUsersTest : SqliteInMemory
{
    private readonly SaltimerDBContext _context;
    private readonly GetAllUsersHandler _handler;

    public GetAllUsersTest() : base()
    {
        _context = new SaltimerDBContext(ContextOptions);
        _handler = new GetAllUsersHandler(_mapper, _mockAuthService.Object, _context);
    }

    [Fact]
    public async void Should_be_abel_to_fetch_all_users()
    {
        var query = new GetAllUsersQuery(); ;

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().BeOfType<List<UserResponseDto>>();

        new List<UserResponseDto>(result).Count.Should().Be(2);
    }


}



