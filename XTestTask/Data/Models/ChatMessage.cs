using System.Text.Json.Serialization;

namespace XTestTask.Data.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int MemberId { get; set; }
        public string Message { get; set; } = null!;  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public virtual Chat Chat { get; set; } = null!;
        [JsonIgnore]
        public virtual ChatMember Member { get; set; } = null!;
    }
}