using System.Net;
using Newtonsoft.Json;
using XTestTask.Data.Models;
using XTestTask.DTO.DAccount;
using XTestTask.DTO.DChat;

namespace XTestTaskTests.IntegrationTests.ChatTests
{
    public class ChatTests : BaseIntegrationTests
    {
        private Chat? _newChat;
        private Account? _newAccount;

        [Test]
        public async Task GetChats_ReturnsOk() 
        {
            // Act
            var response = await _client.GetAsync("/api/v1/chats");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetChat_ReturnsOk()
        {
            // Arrange
            var chat = new ChatDto() 
            {
                Name = Guid.NewGuid().ToString()
            };

            await CreateAccount();
            Assert.NotNull(_newAccount);
            chat.UserId = _newAccount.Id;

            var chatResponse = await _client.PostAsync("/api/v1/chats", HttpContentHelper.Convert(chat));
            var chatObj = JsonConvert.DeserializeObject<Chat>(await chatResponse.Content.ReadAsStringAsync());
            Assert.NotNull(chatObj);

            // Act
            var response = await _client.GetAsync($"/api/v1/chats/{chatObj.Id}");
                
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetChat_WrongChatId_ReturnsNoContent()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"/api/v1/chats/{0}");
                
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task CreateChat_ReturnOk()
        {
            // Arrange
            var chat = new ChatDto() 
            {
                Name = Guid.NewGuid().ToString()
            };

            await CreateAccount();
            Assert.NotNull(_newAccount);
            chat.UserId = _newAccount.Id;

            // Act
            var response = await _client.PostAsync("/api/v1/chats", HttpContentHelper.Convert(chat));
                
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CreateChat_WrongUserId_ReturnsNotFound()
        {
            // Arrange
            var chat = new ChatDto() 
            {
                Name = Guid.NewGuid().ToString(),
                UserId = 0
            };

            // Act
            var response = await _client.PostAsync("/api/v1/chats", HttpContentHelper.Convert(chat));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task CreateChat_SameName_ReturnsBadRequest()
        {
            // Arrange
            var chat = new ChatDto() 
            {
                Name = Guid.NewGuid().ToString()
            };

            await CreateAccount();
            Assert.NotNull(_newAccount);
            chat.UserId = _newAccount.Id;

            // Act
            var successResponse = await _client.PostAsync("/api/v1/chats", HttpContentHelper.Convert(chat));
            var badRequestResponse = await _client.PostAsync("/api/v1/chats", HttpContentHelper.Convert(chat));
                
            // Assert
            successResponse.EnsureSuccessStatusCode();
            Assert.That(badRequestResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task UpdateChat_ReturnsOK()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);
            Assert.NotNull(_newAccount);            

            var updatedChat = new ChatDto() 
            {
                Name = Guid.NewGuid().ToString(),
                UserId = _newAccount.Id
            };

            // Act
            var response = await _client.PutAsync($"/api/v1/chats/{_newChat.Id}", HttpContentHelper.Convert(updatedChat));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task UpdateChat_NotChatCreatorId_ReturnsForbidden()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);

            var updatedChat = new ChatDto() 
            {
                Name = Guid.NewGuid().ToString(),
                UserId = 0
            };

            // Act
            var response = await _client.PutAsync($"/api/v1/chats/{_newChat.Id}", HttpContentHelper.Convert(updatedChat));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task DeleteChat_ReturnsOk()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);
            Assert.NotNull(_newAccount);

            // Act
            var response = await _client.DeleteAsync($"/api/v1/chats/{_newChat.Id}?userId={_newAccount.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task DeleteChat_NotChatCreatorId_ReturnsForbidden()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);

            // Act
            var response = await _client.DeleteAsync($"/api/v1/chats/{_newChat.Id}?userId={0}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        private async Task CreateAccount()
        {
            var user = new AccountDto() 
            {
                Name = Guid.NewGuid().ToString()
            };

            var createAccountRes = await _client.PostAsync("/api/v1/accounts", HttpContentHelper.Convert(user));
            _newAccount = JsonConvert.DeserializeObject<Account>(await createAccountRes.Content.ReadAsStringAsync());
        }

        private async Task CreateChatAndAccount()
        {
            var chat = new ChatDto() 
            {
                Name = Guid.NewGuid().ToString()
            };

            await CreateAccount();
            Assert.NotNull(_newAccount);

            chat.UserId = _newAccount.Id;

            var newChatResponse = await _client.PostAsync("/api/v1/chats", HttpContentHelper.Convert(chat));
            _newChat = JsonConvert.DeserializeObject<Chat>(await newChatResponse.Content.ReadAsStringAsync());
        }
    }
}