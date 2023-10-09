using Microsoft.AspNetCore.SignalR;
using XTestTask.Data.Models;
using XTestTask.DTO;
using XTestTask.DTO.DAccount;
using XTestTask.Services.Interfaces;

namespace XTestTask.Hubs
{
    public class AccountHub : Hub, IAccountHub
    {
        private readonly IAccountService _accountService;

        public AccountHub(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<List<Account>> GetAccounts(Page page)
        {
            return await _accountService.GetAccounts(page);
        }

        public async Task<Account> CreateAccount(AccountDto account)
        {
            return await _accountService.Create(account);
        }
    }
}