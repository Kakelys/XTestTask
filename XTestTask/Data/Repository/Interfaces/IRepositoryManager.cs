namespace XTestTask.Data.Repository.Interfaces
{
    public interface IRepositoryManager
    {
        IAccountRepository Account { get; }
        IChatRepository Chat { get; }
        IChatMemberRepository ChatMember { get; }
        IChatMessageRepository ChatMessage { get; }

        Task BeginTransaction();
        Task Commit();
        Task Rollback();
        Task Save();
    }
}