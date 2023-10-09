using XTestTask.Data.Models;
using XTestTask.DTO.DChat;

namespace XTestTask.Services.Interfaces
{
    public interface IChatHubService
    {
        Task InvokeCloseChat(int chatId);
        Task InvokeReceiveMessage(ChatMessage message);
        Task InvokeUpdateMessage(ChatMessage message);
        Task InvokeDeleteMessage(int chatId, int messageId);
        Task InvokeUpdateChat(Chat chat);
        ConnectedUser? GetConnectedUser(string connectionId);
        ConnectedUser AddUser(string connectionId, int userId, int chatId = 0);
        void RemoveUser(string connectionId);
    }
}