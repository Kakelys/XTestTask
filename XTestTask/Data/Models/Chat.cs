using System.Text.Json.Serialization;

namespace XTestTask.Data.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual Account Creator { get; set; } = null!;
        [JsonIgnore]
        public virtual List<ChatMember> Members { get; set; } = new();
        [JsonIgnore]
        public virtual List<ChatMessage> Messages { get; set; } = new();
    }
}