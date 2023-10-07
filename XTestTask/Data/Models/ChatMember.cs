namespace XTestTask.Data.Models
{
    public class ChatMember
    {
        public int ChatId { get; set; }
        public int AccountId { get; set; } 

        public virtual Chat Chat { get; set; } = null!;
        public virtual Account Account { get; set; } = null!;
        public virtual List<ChatMessage> Messages { get; set; } = new();
    }
}