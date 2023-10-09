using XTestTask.Data.Models;
using XTestTask.DTO;
using XTestTask.DTO.DChat;

namespace XTestTask.Hubs
{
    public interface IChatHub
    {
        Task Connect(int userId);
        Task JoinChat(int userId, int chatId);
        Task<ChatMessage> SendMessage(int userId, int chatId, MessageDto message);
        Task<List<Chat>> GetChats(Page page);
        Task<Chat?> GetChat(int id);
        Task<List<ChatMember>> GetChatMembers(int chatId, Page page);
        Task<List<ChatMessage>> GetChatMessages(int chatId, Page page);
        Task<Chat> CreateChat(ChatDto chatDto);
        Task<Chat> UpdateChat(int chatId, ChatDto chatDto);
        Task<ChatMessage> UpdateMessage(int messageId, MessageDto messageDto);
        Task DeleteChat(int chatId, int userId);
        Task DeleteMessage(int chatId, int messageId, int userId);
    }
}