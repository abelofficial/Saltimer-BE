using System.ComponentModel.DataAnnotations;

namespace Saltimer.Api.Data;

public class MobTimer
{
    [Key]
    public int Id { get; set; }
    
    public string MobName { get; set; }
    
    public int OwnerId { get; set; }
    
    public int RoundTime { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime BreakTime { get; set; }
    
    public DateTime PausedTime { get; set; }
    
    public string SessionUrl { get; set; } = Guid.NewGuid().ToString();
    
    public virtual List<UserMobSession> UserMobSessions {get; set;}
    
}