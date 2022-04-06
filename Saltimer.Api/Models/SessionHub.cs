namespace Saltimer.Api.Models;
public class SessionHub
{
    public DateTime? StartTime { get; set; }
    public DateTime? PausedTime { get; set; }
    public int BreakRound { get; set; } = 2;
    public int TotalRoundCount { get; set; } = 0;
    public string? CurrentDriver { get; set; }
}