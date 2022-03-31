using System.Text.Json.Serialization;

namespace Saltimer.Api.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        [JsonPropertyName("image_url")]
        public string Url { get; set; } = string.Empty;
        [JsonPropertyName("email_address")]
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        
        public virtual List<MobTimer> MobTimers {get; set;}
    }
}
