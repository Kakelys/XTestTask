using Microsoft.AspNetCore.Mvc;
using XTestTask.Controllers.Filters;
using XTestTask.DTO;
using XTestTask.DTO.DChat;
using XTestTask.Services.Interfaces;

namespace XTestTask.Controllers
{
    [ApiController]
    [Route("api/v1/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IChatHubService _chatHubService;

        public ChatController(IChatService chatService, IChatHubService chatHubService)
        {
            _chatService = chatService;
            _chatHubService = chatHubService;
        }

        [HttpGet]
        public async Task<IActionResult> GetChats([FromQuery] Page page)
        {
            return Ok(await _chatService.GetChats(page));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChat(int id)
        {
            return Ok(await _chatService.GetChat(id));
        }

        [HttpGet("{chatId}/members")]
        public async Task<IActionResult> GetChatMembers(int chatId, [FromQuery] Page page)
        {
            return Ok(await _chatService.GetChatMembers(chatId, page));
        }

        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> GetChatMessages(int chatId, [FromQuery] Page page)
        {
            return Ok(await _chatService.GetChatMessages(chatId, page));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChatDto chatDto)
        {
            return Ok(await _chatService.CreateChat(chatDto.UserId, chatDto));
        }

        [HttpPost("{chatId}/members")]
        public async Task<IActionResult> AddMember(int chatId, UserDto user)
        {
            await _chatService.AddMember(chatId, user.UserId);
            return Ok();
        }

        [HttpPost("{chatId}/messages")]
        public async Task<IActionResult> AddMessage(int chatId, MessageDto message)
        {
            var chatMessage = await _chatService.AddMessage(chatId, message.UserId, message.Message);
            await _chatHubService.InvokeReceiveMessage(chatMessage);
            return Ok(chatMessage);
        }

        [HttpPut("{chatId}")]
        [ChatPermissionFilter("chatDto", "chatId", "UserId")]
        public async Task<IActionResult> Update(int chatId, ChatDto chatDto)
        {
            var updatedChat = await _chatService.UpdateChat(chatId, chatDto);
            await _chatHubService.InvokeUpdateChat(updatedChat);
            return Ok(updatedChat);
        }

        [HttpPut("{chatId}/messages/{messageId}")]
        [MessagePermissionFilter("messageDto", "messageId", "UserId")]
        public async Task<IActionResult> UpdateMessage(int chatId, int messageId, MessageDto messageDto)
        {
            var updatedMessage = await _chatService.UpdateMessage(messageId, messageDto);
            await _chatHubService.InvokeUpdateMessage(updatedMessage);
            return Ok(updatedMessage);
        }

        [HttpDelete("{chatId}")]
        [ChatPermissionFilter]
        public async Task<IActionResult> Delete(int chatId, [FromQuery] int userId)
        {
            await _chatService.DeleteChat(chatId);
            await _chatHubService.InvokeCloseChat(chatId);
            return Ok();
        }

        [HttpDelete("{chatId}/messages/{messageId}")]
        [MessagePermissionFilter]
        public async Task<IActionResult> DeleteMessage(int chatId, int messageId, [FromQuery] int userId)
        {
            await _chatService.DeleteMessage(messageId);
            await _chatHubService.InvokeDeleteMessage(chatId, messageId);
            return Ok();
        }

        [HttpDelete("{chatId}/members/{userId}")]
        public async Task<IActionResult> DeleteMember(int chatId, int userId)
        {
            await _chatService.DeleteMember(chatId, userId);
            return Ok();
        }
    }
}