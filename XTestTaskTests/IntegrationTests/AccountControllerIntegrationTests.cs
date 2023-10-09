using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using XTestTask.Data;
using XTestTask.DTO.DAccount;

namespace XTestTaskTests.IntegrationTests
{
    public class AccountControllerIntegrationTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory<Program>();

            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GetAccounts_ReturnsOkResult() 
        {
            // Act
            var response = await _client.GetAsync("/api/v1/accounts");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CreateUser_ReturnOkResult()
        {
            // Arrange
            var user = new AccountDto() 
            {
                Name = Guid.NewGuid().ToString()
            };

            // Act
            var response = await _client.PostAsync("/api/v1/accounts", HttpContentHelper.Convert(user));
                
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CreateUser_SameName_ReturnsBadRequest()
        {
            // Arrange
            var user = new AccountDto() 
            {
                Name = Guid.NewGuid().ToString()
            };

            // Act
            var response = await _client.PostAsync("/api/v1/accounts", HttpContentHelper.Convert(user));
            var response2 = await _client.PostAsync("/api/v1/accounts", HttpContentHelper.Convert(user));
                
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TearDown]
        public void TearDown() 
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}