using System.Collections.Generic;
using efCdCollection.Api.Controllers;
using efCdCollection.Api.Dtos;
using efCdCollection.Api.Repository;
using Moq;
using Xunit;

namespace efCdCollection.Tests;

public class CreateSessionTest
{


    public CreateSessionTest()
    {

        setupMock();
    }

    private void setupMock()
    {

    }

    [Fact]
    public void HAPPY_Should_call_GetAllCDs_one_time()
    {
        // Arrange


    }

}