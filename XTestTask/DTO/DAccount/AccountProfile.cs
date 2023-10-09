using AutoMapper;
using XTestTask.Data.Models;

namespace XTestTask.DTO.DAccount
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountDto, Account>();
        }
    }
}