using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Dto;
using Saltimer.Api.Models;

namespace Saltimer.Api.Hubs;


public class MobTimerHub : Hub
{
    private readonly IDictionary<string, string> _sessionUsers;
    private IDictionary<string, SessionHub> _sessionHubs;
    protected readonly IMapper _mapper;
    protected readonly SaltimerDBContext _db;

    public MobTimerHub(SaltimerDBContext db, IMapper mapper, IDictionary<string, string> sessionUsers, IDictionary<string, SessionHub> sessionHubs)
    {
        _db = db;
        _mapper = mapper;
        _sessionHubs = sessionHubs;
        _sessionUsers = sessionUsers;
    }

    public async Task JoinSession(JoinSessionRequest request)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, request.Uuid.ToString());

        var targetUser = await _db.User.FindAsync(request.UserId);

        _sessionUsers[targetUser.Username] = request.Uuid.ToString();

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

        await Clients
            .GroupExcept(request.Uuid.ToString(), Context.ConnectionId)
            .SendAsync("NotifyNewMember", $"{targetUser.Username} has joined session.");

        await SendUsersConnected(request.Uuid.ToString());
    }

    public async Task SendUsersConnected(string groupId)
    {
        var usersNames = _sessionUsers
               .Where(c => c.Value == groupId)
               .Select(c => c.Key);

        var users = await _db.User.Where(u => usersNames.Contains(u.Username)).ToListAsync();

        var response = _mapper.Map<IEnumerable<UserResponseDto>>(users);
        await Clients
            .Group(groupId)
            .SendAsync("ReceiveOnlineMember", response);
    }

}
