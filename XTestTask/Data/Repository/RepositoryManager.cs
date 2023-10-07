using XTestTask.Data.Repository.Interfaces;

namespace XTestTask.Data.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;

        public IAccountRepository Account { get; }

        public IChatRepository Chat { get; }

        public IChatMemberRepository ChatMember { get; }

        public IChatMessageRepository ChatMessage { get; }

        public RepositoryManager(
            AppDbContext context,
            IAccountRepository account,
            IChatRepository chat,
            IChatMemberRepository chatMember,
            IChatMessageRepository chatMessage)
        {
            _context = context;
            Account = account;
            Chat = chat;
            ChatMember = chatMember;
            ChatMessage = chatMessage;
        }

        public async Task BeginTransaction() =>
            await _context.Database.BeginTransactionAsync();
        

        public async Task Commit() =>
            await _context.Database.CommitTransactionAsync();

        public async Task Rollback() => 
            await _context.Database.RollbackTransactionAsync();

        public async Task Save() =>
            await _context.SaveChangesAsync();
    }
}