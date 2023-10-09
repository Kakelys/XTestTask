using Microsoft.AspNetCore.Mvc;
using Moq;
using XTestTask.Controllers;
using XTestTask.Data.Models;
using XTestTask.DTO;
using XTestTask.DTO.DAccount;
using XTestTask.Services.Interfaces;
using XTestTask.Data.Repository.Extensions;

namespace XTestTaskTests.UnitTests;

public class Tests
{
    private AccountController _controller;
    private Mock<IAccountService> _serviceMock;

    [SetUp]
    public void Setup()
    {
        var accounts = new List<Account>
        {
            new Account
            {
                Id = 1,
                Name = "Test1",
            },
            new Account
            {
                Id = 2,
                Name = "Test2",
            },
            new Account
            {
                Id = 3,
                Name = "Test3",
            },
            new Account
            {
                Id = 4,
                Name = "Test4",
            },
            new Account
            {
                Id = 5,
                Name = "Test5",
            },
        };

        _serviceMock = new Mock<IAccountService>();
        _serviceMock.Setup(x => x.GetAccounts(It.IsAny<Page>())).ReturnsAsync((Page page) => 
        {
            return accounts
                .AsQueryable()
                .OrderBy(x => x.Id)
                .TakePage(page)
                .ToList();
        });
        _serviceMock.Setup(x => x.Create(It.IsAny<AccountDto>())).ReturnsAsync((AccountDto account) =>
        {
            var newAccount = new Account
            {
                Id = accounts.Count + 1,
                Name = account.Name,
            };
            accounts.Add(newAccount);
            return newAccount;
        });

        _controller = new AccountController(_serviceMock.Object);

    }

    [Test]
    public async Task GetAccounts_ReturnsOkResult()
    {
        // Arrange
        var page = new Page();

        //Act
        var result = await _controller.GetAccounts(page);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task GetAccounts_PageLimit_ReturnsOkResult_WithCorrectCount()
    {
        // Arrange
        var page = new Page
        {
            Limit = 2,
        };

        //Act
        var result = await _controller.GetAccounts(page);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = result as OkObjectResult;

        Assert.NotNull(okResult);
        var resultAccounts = okResult.Value as List<Account>;

        Assert.NotNull(resultAccounts);
        Assert.That(resultAccounts.Count, Is.EqualTo(page.Limit));
    }

    [Test]
    public async Task GetAccounts_PageLimit_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        var page = new Page
        {
            Limit = 0,
        };

        //Act
        var result = await _controller.GetAccounts(page);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = result as OkObjectResult;

        Assert.NotNull(okResult);
        var resultAccounts = okResult.Value as List<Account>;

        Assert.NotNull(resultAccounts);
        Assert.That(resultAccounts.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetAccounts_PageOffset_ReturnsOkResult_WithCorrectId()
    {
        // Arrange
        var page = new Page
        {
            Offset = 1,
        };

        //Act
        var result = await _controller.GetAccounts(page);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = result as OkObjectResult;

        Assert.NotNull(okResult);
        var resultAccounts = okResult.Value as List<Account>;

        Assert.NotNull(resultAccounts);
        var firstAccount = resultAccounts.First();
        Assert.That(firstAccount.Id, Is.EqualTo(page.Offset + 1));
    }

    [Test]
    public async Task CreateAccount_ReturnsOkResult()
    {
        // Arrange
        var account = new AccountDto
        {
            Name = "Test",
        };

        //Act
        var result = await _controller.CreateAccount(account);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
}