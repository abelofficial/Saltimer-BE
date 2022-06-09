using System;
using Xunit;
using System.Collections.Generic;
using FluentAssertions;
using Saltimer.Api.Models;

namespace Saltimer.Api.Tests;

public class SaltimerTests
{
    [Fact]
    public void Test1()
    {
        // arrange
        int two = 2;
        int three = 3;

        // act
        var result = two + three;

        // assert
        result.Should().Be(5);
    }

    [Fact]
    public void should_be_able_to_create_user()
    {
        // arrange
        var user = new User() { Id = 5, Username = "Rambo", FirstName = "Sylvester", LastName = "Stallone", EmailAddress = "Stallone@hotmail.com" };

        // act
        string email = user.EmailAddress;

        // assert
        email.Should().Be("Stallone@hotmail.com");
    }

    [Fact]
    public void should_create_new_mobtimersession()
    {
        // arrange
        var user = new User() { Id = 10, Username = "Ram", FirstName = "Bob", LastName = "Book", EmailAddress = "bob_book@email.com" };

        var session = new MobTimerSession()
        {
            Id = 3,
            DisplayName = "JamNet",
            RoundTime = 10,
            BreakTime = 30,
            Owner = user,
        };

        // act
        var sessionId = session.Id;

        // assert
        sessionId.Should().Be(3);
    }

    [Fact]
    public void should_have_owner_added_to_mobtimersession()
    {
        // arrange
        var user = new User() { Id = 7, Username = "Don", FirstName = "Ron", LastName = "Son", EmailAddress = "Gum@email.com" };

        var session = new MobTimerSession()
        {
            Id = 3,
            DisplayName = "JamNet",
            RoundTime = 10,
            BreakTime = 30,
            Owner = user,
        };

        // act
        var sessionOwner = session.Owner;

        // assert
        sessionOwner.FirstName.Should().Be("Ron");
    }
}
