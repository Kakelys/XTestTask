using XTestTask.Data.Models;
using XTestTask.Data.Repository.Interfaces;

namespace XTestTask.Data.Repository.Implements
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context)
        {
        }
    }
}