using System.Net;
using Newtonsoft.Json;
using XTestTask.Data.Models;
using XTestTask.DTO.DAccount;
using XTestTask.DTO.DChat;

namespace XTestTaskTests.IntegrationTests.ChatTests
{
    public class ChatMessageTests : BaseIntegrationTests
    {
        private Chat? _newChat;
        private Account? _newAccount;

        [Test]
        public async Task GetChatMessages_ReturnsOk()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);
            Assert.NotNull(_newAccount);

            // Act
            var response = await _client.GetAsync($"/api/v1/chats/{_newChat.Id}/messages");

            // Arrange
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task AddMessage_ReturnsOk()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);
            Assert.NotNull(_newAccount);

            var message = new MessageDto() 
            {
                Message = Guid.NewGuid().ToString(),
                UserId = _newAccount.Id
            };

            // Act
            var response = await _client.PostAsync($"/api/v1/chats/{_newChat.Id}/messages", HttpContentHelper.Convert(message));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task AddMessage_WrongUserId_ReturnsNotFound()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);

            var message = new MessageDto() 
            {
                Message = Guid.NewGuid().ToString(),
                UserId = 0
            };

            // Act
            var response = await _client.PostAsync($"/api/v1/chats/{_newChat.Id}/messages", HttpContentHelper.Convert(message));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task AddMessage_WrongChatId_ReturnsNotFound()
        {
            // Arrange
            var message = new MessageDto() 
            {
                Message = Guid.NewGuid().ToString(),
                UserId = 0
            };

            // Act
            var response = await _client.PostAsync($"/api/v1/chats/{0}/messages", HttpContentHelper.Convert(message));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UpdateMessage_ReturnsOk()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);
            Assert.NotNull(_newAccount);

            var message = new MessageDto() 
            {
                Message = Guid.NewGuid().ToString(),
                UserId = _newAccount.Id
            };
            var newMessageResponse = await _client.PostAsync($"/api/v1/chats/{_newChat.Id}/messages", HttpContentHelper.Convert(message));
            var newMessage = JsonConvert.DeserializeObject<ChatMessage>(await newMessageResponse.Content.ReadAsStringAsync());
            Assert.NotNull(newMessage);

            // Act
            var response = await _client.PutAsync($"/api/v1/chats/{_newChat.Id}/messages/{newMessage.Id}", HttpContentHelper.Convert(message));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task UpdateMessage_NotCreatorId_ReturnsForbidden()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);
            Assert.NotNull(_newAccount);

            var message = new MessageDto() 
            {
                Message = Guid.NewGuid().ToString(),
                UserId = _newAccount.Id
            };
            var newMessageResponse = await _client.PostAsync($"/api/v1/chats/{_newChat.Id}/messages", HttpContentHelper.Convert(message));
            var newMessage = JsonConvert.DeserializeObject<ChatMessage>(await newMessageResponse.Content.ReadAsStringAsync());
            Assert.NotNull(newMessage);

            message.UserId = 0;

            // Act
            var response = await _client.PutAsync($"/api/v1/chats/{_newChat.Id}/messages/{newMessage.Id}", HttpContentHelper.Convert(message));

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        private async Task CreateChatAndAccount()
        {
            var chat = new ChatDto() 
            {
                Name = Guid.NewGuid().ToString()
            };

            var user = new AccountDto() 
            {
                Name = Guid.NewGuid().ToString()
            };

            var createAccountRes = await _client.PostAsync("/api/v1/accounts", HttpContentHelper.Convert(user));
            _newAccount = JsonConvert.DeserializeObject<Account>(await createAccountRes.Content.ReadAsStringAsync());
            Assert.NotNull(_newAccount);
            chat.UserId = _newAccount.Id;

            var newChatResponse = await _client.PostAsync("/api/v1/chats", HttpContentHelper.Convert(chat));
            _newChat = JsonConvert.DeserializeObject<Chat>(await newChatResponse.Content.ReadAsStringAsync());
        }
    }
}