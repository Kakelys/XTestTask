namespace XTestTask.Data.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; } = null!;

        public virtual Account Creator { get; set; } = null!;
        public virtual List<ChatMember> Members { get; set; } = new();
        public virtual List<ChatMessage> Messages { get; set; } = new();
    }
}