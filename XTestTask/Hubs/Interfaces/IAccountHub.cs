using XTestTask.Data.Models;
using XTestTask.DTO;
using XTestTask.DTO.DAccount;

namespace XTestTask.Hubs
{
    public interface IAccountHub
    {
        Task<List<Account>> GetAccounts(Page page);
        Task<Account> CreateAccount(AccountDto account);
    }
}