using XTestTask.Data.Models;
using XTestTask.Data.Repository.Interfaces;

namespace XTestTask.Data.Repository.Implements
{
    public class ChatMessageRepository : RepositoryBase<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(AppDbContext context) : base(context)
        {
        }
    }
}