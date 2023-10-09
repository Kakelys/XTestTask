using Microsoft.AspNetCore.Mvc;
using XTestTask.DTO;
using XTestTask.DTO.DAccount;
using XTestTask.Services.Interfaces;

namespace XTestTask.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService service)
        {
            _accountService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts([FromQuery] Page page)
        {
            return Ok(await _accountService.GetAccounts(page));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountDto account)
        {
            return Ok(await _accountService.Create(account));
        }
    }
}