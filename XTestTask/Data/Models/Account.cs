namespace XTestTask.Data.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual List<Chat> CreatedChats { get; set; } = new();
        public virtual List<ChatMember> Chats { get; set; } = new();
    }
}