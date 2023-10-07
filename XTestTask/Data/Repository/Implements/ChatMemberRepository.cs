using XTestTask.Data.Models;
using XTestTask.Data.Repository.Interfaces;

namespace XTestTask.Data.Repository.Implements
{
    public class ChatMemberRepository : RepositoryBase<ChatMember>, IChatMemberRepository
    {
        public ChatMemberRepository(AppDbContext context) : base(context)
        {
        }
    }
}