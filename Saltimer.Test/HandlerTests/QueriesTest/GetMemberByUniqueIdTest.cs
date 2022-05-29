using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using efCdCollection.Tests.Data;
using FluentAssertions;
using Saltimer.Api.Handlers;
using Saltimer.Api.Queries;
using Xunit;

namespace Saltimer.Test.HandlerTests;

public class GetMemberByUniqueIdTest : SqliteInMemory
{
    private readonly SaltimerDBContext _context;
    private readonly GetMemberByUniqueIdHandler _handler;

    public GetMemberByUniqueIdTest() : base()
    {
        _context = new SaltimerDBContext(ContextOptions);
        _handler = new GetMemberByUniqueIdHandler(_mapper, _mockAuthService.Object, _context);
    }

    [Fact]
    public async void Should_throw_not_found_exception_when_UniqueId_is_not_found()
    {
        var query = new GetMemberByUniqueIdQuery()
        {
            UniqueId = Guid.NewGuid(),
        };

        var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _handler.Handle(query, CancellationToken.None));
        ((HttpRequestException)exception).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


}



