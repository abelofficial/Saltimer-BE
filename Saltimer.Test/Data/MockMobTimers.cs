using System.Collections.Generic;
using Saltimer.Api.Models;

namespace Saltimer.Test.Mocks;

public static class MockMobTimers
{

    public static MobTimerSession GetMobTimer(User owner) => new MobTimerSession()
    {
        DisplayName = "Test session",
        RoundTime = 30,
        BreakTime = 30,
        Owner = owner,
        Members = new List<SessionMember>(),
    };

    public static SessionMember GetMobTimerMember(User user, int turn, MobTimerSession session) => new SessionMember()
    {
        Turn = turn,
        User = user,
        Session = session,

    };

}



