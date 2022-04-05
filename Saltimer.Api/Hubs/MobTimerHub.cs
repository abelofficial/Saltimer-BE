using Microsoft.AspNetCore.SignalR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Hubs;

public class MobTimerHub : Hub
{
    private readonly IDictionary<string, MobtimerConnection> _connections;

    public async Task JoinRoom(MobtimerConnection mobtimerConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, mobtimerConnection.Uuid);

        await Clients
            .Group(mobtimerConnection.Uuid)
            .SendAsync("ReceiveMessage", "Me", $"{mobtimerConnection.UserId} has joined {mobtimerConnection.Uuid}");
    }

}
