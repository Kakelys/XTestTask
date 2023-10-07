using XTestTask.Data.Models;
using XTestTask.Data.Repository.Interfaces;

namespace XTestTask.Data.Repository.Implements
{
    public class ChatRepository : RepositoryBase<Chat>, IChatRepository
    {
        public ChatRepository(AppDbContext context) : base(context)
        {
        }
    }
}