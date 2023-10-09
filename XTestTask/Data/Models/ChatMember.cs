using System.Text.Json.Serialization;

namespace XTestTask.Data.Models
{
    public class ChatMember
    {
        public int ChatId { get; set; }
        public int AccountId { get; set; } 

        [JsonIgnore]
        public virtual Chat Chat { get; set; } = null!;
        [JsonIgnore]
        public virtual Account Account { get; set; } = null!;
        [JsonIgnore]
        public virtual List<ChatMessage> Messages { get; set; } = new();
    }
}