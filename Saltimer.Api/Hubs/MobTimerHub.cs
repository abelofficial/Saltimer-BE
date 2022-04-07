using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Models;

namespace Saltimer.Api.Hubs;


public class MobTimerHub : Hub
{
    private readonly IDictionary<string, SessionHubUsers> _sessionUsers;
    private IDictionary<string, SessionHub> _sessionHubs;
    protected readonly IMapper _mapper;
    protected readonly SaltimerDBContext _db;

    public MobTimerHub(SaltimerDBContext db, IMapper mapper, IDictionary<string, SessionHubUsers> sessionUsers, IDictionary<string, SessionHub> sessionHubs)
    {
        _db = db;
        _mapper = mapper;
        _sessionHubs = sessionHubs;
        _sessionUsers = sessionUsers;
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        if (_sessionUsers.TryGetValue(Context.ConnectionId, out SessionHubUsers userConnection))
        {
            _sessionUsers.Remove(Context.ConnectionId);

            if (!_sessionUsers.Any(sh => sh.Value.ConnectionId.Equals(userConnection.ConnectionId.ToString())))
            {
                _sessionHubs.Remove(userConnection.ConnectionId.ToString());
            }

            var t = Task.Run(async () =>
            {
                await Clients.Group(userConnection.ConnectionId).SendAsync("ReceiveUserLeaveSession", new
                {
                    Title = "Info",
                    message = $"{userConnection.Username} has left session."
                });
                await SendUsersConnected(userConnection.ConnectionId);
            });
            t.Wait();
        }

        return base.OnDisconnectedAsync(exception);
    }


    public async Task JoinSession(JoinSessionRequest request)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, request.Uuid.ToString());

        var targetUser = await _db.User.FindAsync(request.UserId);

        _sessionUsers[Context.ConnectionId] = new SessionHubUsers()
        {
            ConnectionId = request.Uuid.ToString(),
            Username = targetUser.Username,
        };

        if (!_sessionHubs.Any(sh => sh.Key.Equals(request.Uuid.ToString())))
        {
            _sessionHubs[request.Uuid.ToString()] = new SessionHub()
            {
                CurrentDriver = targetUser.Username
            };
        }

        var response = await _db.MobTimerSession
            .Where(mt => mt.UniqueId.Equals(request.Uuid.ToString()))
            .Select(mt => _mapper.Map<SessionResponse>(mt))
            .SingleOrDefaultAsync();

        var targetMembers = await _db.SessionMember
            .Include(mt => mt.User)
            .Where(mt => mt.Session.UniqueId.Equals(request.Uuid.ToString()))
            .Select(mt => _mapper.Map<UserResponseDto>(mt.User))
            .ToListAsync();


        response.Users = targetMembers;

        await Clients
            .Caller
            .SendAsync("ReceiveSession", response, _sessionHubs[request.Uuid.ToString()]);

        await NotifyClient(request.Uuid.ToString(), "Info", $"{targetUser.Username} has joined session.");

        await SendUsersConnected(request.Uuid.ToString());
    }

    public async Task PlayTimer(SessionUpdateRequest request)
    {
        _sessionHubs[request.Uuid.ToString()].IsPaused = false;
        _sessionHubs[request.Uuid.ToString()].PausedTime = _sessionHubs[request.Uuid.ToString()].StartTime;
        _sessionHubs[request.Uuid.ToString()].StartTime = DateTime.Now;

        await Clients
            .Group(request.Uuid.ToString())
            .SendAsync("ReceiveSessionUpdate", _sessionHubs[request.Uuid.ToString()]);
    }

    public async Task NextDriver(SessionUpdateRequest request)
    {
        _sessionHubs[request.Uuid.ToString()].IsPaused = true;
        _sessionHubs[request.Uuid.ToString()].PausedTime = null;
        _sessionHubs[request.Uuid.ToString()].StartTime = null;
        _sessionHubs[request.Uuid.ToString()].TotalRoundCount = _sessionHubs[request.Uuid.ToString()].TotalRoundCount + 1;

        await Clients
            .Group(request.Uuid.ToString())
            .SendAsync("ReceiveSessionUpdate", _sessionHubs[request.Uuid.ToString()]);
    }

    public async Task PauseTimer(SessionUpdateRequest request)
    {
        _sessionHubs[request.Uuid.ToString()].IsPaused = true;
        _sessionHubs[request.Uuid.ToString()].PausedTime = DateTime.Now; ;

        await Clients
            .Group(request.Uuid.ToString())
            .SendAsync("ReceiveSessionUpdate", _sessionHubs[request.Uuid.ToString()]);
    }

    public async Task SendUsersConnected(string groupId)
    {
        var usersNames = _sessionUsers
               .Where(c => c.Value.ConnectionId == groupId)
               .Select(c => c.Value.Username);

        var users = await _db.User.Where(u => usersNames.Contains(u.Username)).ToListAsync();

        var response = _mapper.Map<IEnumerable<UserResponseDto>>(users);
        await Clients
            .Group(groupId)
            .SendAsync("ReceiveOnlineMember", response);
    }

    public async Task NotifyClient(string excludeClient, string title, string message)
    {
        await Clients
           .GroupExcept(excludeClient, Context.ConnectionId)
           .SendAsync("NotifyClient", new { title, message });
    }

}
