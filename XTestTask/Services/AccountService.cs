using AutoMapper;
using Microsoft.EntityFrameworkCore;
using XTestTask.Data.Models;
using XTestTask.Data.Repository.Extensions;
using XTestTask.Data.Repository.Interfaces;
using XTestTask.DTO;
using XTestTask.DTO.DAccount;
using XTestTask.Middlewares.ExceptionMiddleware.Exceptions;
using XTestTask.Services.Interfaces;

namespace XTestTask.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepositoryManager _rep;
        private readonly IMapper _mapper;

        public AccountService(
            IRepositoryManager rep,
            IMapper mapper)
        {
            _rep = rep;
            _mapper = mapper;
        }

        
        public async Task<List<Account>> GetAccounts(Page page)
        {
            return await _rep.Account
                .FindAll()
                .OrderBy(x => x.Id)
                .TakePage(page)
                .ToListAsync();
        }

        public async Task<Account> Create(AccountDto account)
        {
            if(await _rep.Account.FindByCondition(x => x.Name == account.Name).AnyAsync())
                throw new BadRequestException("Account with this name already exists");

            var accountEntity = _rep.Account.Create(_mapper.Map<Account>(account));
            await _rep.Save();

            return accountEntity;
        }

        public async Task<Account> Update(int id, AccountDto account)
        {
            if (await _rep.Account.FindByCondition(x => x.Name == account.Name && x.Id != id).AnyAsync())
                throw new BadRequestException("Account with this name already exists");

            var accountEntity = _rep.Account
                .FindByCondition(x => x.Id == id, true)
                .FirstOrDefault() ?? throw new NotFoundException("Account not found");

            _mapper.Map(account, accountEntity);
            await _rep.Save();

            return accountEntity;
        }

        public async Task Delete(int id)
        {
            var accountEntity = _rep.Account
                .FindByCondition(x => x.Id == id)
                .FirstOrDefault() ?? throw new NotFoundException("Account not found");

            _rep.Account.Delete(accountEntity);
            await _rep.Save();
        }
    }
}