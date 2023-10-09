using XTestTask.Data.Models;
using XTestTask.DTO;
using XTestTask.DTO.DAccount;

namespace XTestTask.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<Account>> GetAccounts(Page page); 
        Task<Account> Create(AccountDto account);
        Task<Account> Update(int id, AccountDto account);
        Task Delete(int id);
    }
}