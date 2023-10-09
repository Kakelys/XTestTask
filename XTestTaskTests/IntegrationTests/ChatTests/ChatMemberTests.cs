using System.Net;
using Newtonsoft.Json;
using XTestTask.Data.Models;
using XTestTask.DTO.DAccount;
using XTestTask.DTO.DChat;

namespace XTestTaskTests.IntegrationTests.ChatTests
{
    public class ChatMemberTests : BaseIntegrationTests
    {
        private Chat? _newChat;
        private Account? _newAccount;

        [Test]
        public async Task GetChatMembers_ReturnsOk_WithOneMember()
        {
            // Arrange
            await CreateChatAndAccount();
            Assert.NotNull(_newChat);
            Assert.NotNull(_newAccount);

            // Act
            var response = await _client.GetAsync($"/api/v1/chats/{_newChat.Id}/members");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var members = JsonConvert.DeserializeObject<List<Account>>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(members);
            Assert.That(members.Count, Is.EqualTo(1));
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