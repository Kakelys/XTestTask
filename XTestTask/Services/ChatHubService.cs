using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using XTestTask.Data.Models;
using XTestTask.Data.Repository.Interfaces;
using XTestTask.DTO.DChat;
using XTestTask.Hubs;
using XTestTask.Services.Interfaces;

namespace XTestTask.Services
{
    public class ChatHubService : IChatHubService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IRepositoryManager _rep;
        private readonly static Dictionary<string, ConnectedUser> _connectedUsers = new();

        public ChatHubService(IHubContext<ChatHub> hubContext, IRepositoryManager rep)
        {
            _hubContext = hubContext;
            _rep = rep;
        }

        public async Task InvokeCloseChat(int chatId)
        {
            await _hubContext.Clients.Group(chatId.ToString()).SendAsync("CloseChat", chatId);
        }

        public async Task InvokeReceiveMessage(ChatMessage message) 
        {
            var chatMemberIds = await _rep.ChatMember
                .FindByCondition(c => c.ChatId == message.ChatId)
                .Select(x => x.AccountId)
                .ToListAsync();

            var connectionIds = _connectedUsers
                .Where(x => chatMemberIds.Contains(x.Value.AccountId))
                .Select(x => x.Key)
                .ToList();

            foreach (var connectionId in connectionIds)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
        }

        public async Task InvokeUpdateMessage(ChatMessage message)
        {
            await _hubContext.Clients.Group(message.ChatId.ToString()).SendAsync("UpdateMessage", message);
        }

        public async Task InvokeDeleteMessage(int chatId, int messageId)
        {
            await _hubContext.Clients.Group(chatId.ToString()).SendAsync("DeleteMessage", messageId);
        }

        public async Task InvokeUpdateChat(Chat chat)
        {
            await _hubContext.Clients.Group(chat.Id.ToString()).SendAsync("UpdateChat", chat);
        }

        public ConnectedUser? GetConnectedUser(string connectionId)
        {
            _connectedUsers.TryGetValue(connectionId, out var connectedUser);
                
            return connectedUser;
        }

        public ConnectedUser AddUser(string connectionId, int userId, int chatId = 0)
        {
            var connectedUser = new ConnectedUser(userId, chatId);
            _connectedUsers.Add(connectionId, connectedUser);

            return connectedUser;
        }

        public void RemoveUser(string connectionId)
        {
            _connectedUsers.Remove(connectionId);
        }
    }
}