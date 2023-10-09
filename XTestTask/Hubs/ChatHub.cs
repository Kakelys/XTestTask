using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using XTestTask.Data.Models;
using XTestTask.Data.Repository.Interfaces;
using XTestTask.DTO;
using XTestTask.DTO.DChat;
using XTestTask.Middlewares.ExceptionMiddleware.Exceptions;
using XTestTask.Services.Interfaces;

namespace XTestTask.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly IChatService _chatService;
        private readonly IChatHubService _chatHubService;
        private readonly IRepositoryManager _rep;

        public ChatHub(
            IChatService chatService, 
            IChatHubService chatHubService,
            IRepositoryManager rep)
        {
            _chatService = chatService;
            _chatHubService = chatHubService;
            _rep = rep;
        }

        public Task Connect(int userId)
        {
            _chatHubService.AddUser(Context.ConnectionId, userId);
            return Task.CompletedTask;
        }

        public async Task JoinChat(int userId, int chatId)
        {
            await _chatService.AddMember(chatId, userId);

            var user = _chatHubService.GetConnectedUser(Context.ConnectionId);
            if(user == null)
                user = _chatHubService.AddUser(Context.ConnectionId, userId, chatId);
            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.CurrentChatId.ToString());
            user.CurrentChatId = chatId;
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task<ChatMessage> SendMessage(int userId, int chatId, MessageDto message)
        {
            var chatMessage = await _chatService.AddMessage(chatId, userId, message.Message);
            await _chatHubService.InvokeReceiveMessage(chatMessage);

            return chatMessage;
        }

        public async Task<List<Chat>> GetChats(Page page)
        {
            return await _chatService.GetChats(page);
        }

        public Task<Chat?> GetChat(int id)
        {
            return _chatService.GetChat(id);
        }

        public Task<List<ChatMember>> GetChatMembers(int chatId, Page page)
        {
            return _chatService.GetChatMembers(chatId, page);
        }

        public Task<List<ChatMessage>> GetChatMessages(int chatId, Page page)
        {
            return _chatService.GetChatMessages(chatId, page);
        }

        public async Task<Chat> CreateChat(ChatDto chatDto)
        {
            return await _chatService.CreateChat(chatDto.UserId, chatDto);            
        }

        public async Task<Chat> UpdateChat(int chatId, ChatDto chatDto)
        {
            if(!await ChatPermissionCheck(chatDto.UserId, chatId))
                throw new ForbiddenException("You are not creator of this chat to perform this action");

            var chat = await _chatService.UpdateChat(chatId, chatDto);
            await _chatHubService.InvokeUpdateChat(chat);

            return chat;
        }

        public async Task<ChatMessage> UpdateMessage(int messageId, MessageDto messageDto)
        {
            if(!await MessagePermissionCheck(messageDto.UserId, messageId))
                throw new ForbiddenException("You are not creator of this message to perform this action");

            var chatMessage = await _chatService.UpdateMessage(messageId, messageDto);
            await _chatHubService.InvokeUpdateMessage(chatMessage);

            return chatMessage;
        }

        public async Task DeleteChat(int chatId, int userId)
        {
            if(!await ChatPermissionCheck(userId, chatId))
                throw new ForbiddenException("You are not creator of this chat to perform this action");

            await _chatService.DeleteChat(chatId);
            await _chatHubService.InvokeCloseChat(chatId);
        }

        public async Task DeleteMessage(int chatId, int messageId, int userId)
        {
            if(!await MessagePermissionCheck(userId, messageId))
                throw new ForbiddenException("You are not creator of this message to perform this action");

            await _chatService.DeleteMessage(messageId);
            await _chatHubService.InvokeDeleteMessage(chatId, messageId);
        }

        private async Task<bool> ChatPermissionCheck(int userId, int chatId)
        {
            if(await _rep.Chat.FindByCondition(c => c.Id == chatId && c.CreatorId == userId).AnyAsync())
                return true;

            return false;
        }

        private async Task<bool> MessagePermissionCheck(int userId, int messageId)
        {
            if(await _rep.ChatMessage.FindByCondition(m => m.Id == messageId && m.MemberId == userId).AnyAsync())
                return true;

            return false;
        }
    }
}