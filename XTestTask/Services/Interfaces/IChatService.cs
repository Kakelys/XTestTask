using XTestTask.Data.Models;
using XTestTask.DTO;
using XTestTask.DTO.DChat;

namespace XTestTask.Services.Interfaces
{
    public interface IChatService
    {
        Task<List<Chat>> GetChats(Page page);
        Task<Chat?> GetChat(int id);
        Task<List<ChatMember>> GetChatMembers(int chatId, Page page);
        Task<List<ChatMessage>> GetChatMessages(int chatId, Page page);
        Task<Chat> CreateChat(int userId, ChatDto chatDto);
        Task AddMember(int chatId, int accountId);
        Task<ChatMessage> AddMessage(int chatId, int memberId, string message);
        Task<Chat> UpdateChat(int chatId, ChatDto chatDto);
        Task<ChatMessage> UpdateMessage(int messageId, MessageDto messageDto);
        Task DeleteChat(int chatId);
        Task DeleteMessage(int messageId);
        Task DeleteMember(int chatId, int userId);
    }
}