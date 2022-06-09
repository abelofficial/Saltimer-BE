using System.Collections.Generic;
using Saltimer.Api.Models;

namespace Saltimer.Test.Mocks;

public static class MockUsers
{

    public static User GetAdminUser() => new User()
    {
        Username = "Admin",
        FirstName = "Admin",
        LastName = "Admin",
        EmailAddress = "Admin",
        PasswordHash = new byte[0],
        PasswordSalt = new byte[0],
        MobTimers = new List<MobTimerSession>(),
    };

    public static User GetUniqueUser(int id) => new User()
    {
        Username = $"user-{id}",
        FirstName = $"user-{id}",
        LastName = $"user-{id}",
        EmailAddress = $"user-{id}",
        PasswordHash = new byte[0],
        PasswordSalt = new byte[0],
        MobTimers = new List<MobTimerSession>(),
    };



}



