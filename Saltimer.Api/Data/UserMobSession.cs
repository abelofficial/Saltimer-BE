using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saltimer.Api.Data;

public class UserMobSession
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("UserId")]
    public virtual int UserId { get; set; }

    [ForeignKey("MobTimerId")]
    public virtual int MobTimerId { get; set; }
    
    public int Turn { get; set; }
}