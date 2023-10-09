using System.Text.Json.Serialization;

namespace XTestTask.Data.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual List<Chat> CreatedChats { get; set; } = new();
        [JsonIgnore]
        public virtual List<ChatMember> Chats { get; set; } = new();
    }
}