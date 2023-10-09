using AutoMapper;
using Microsoft.EntityFrameworkCore;
using XTestTask.Data.Models;
using XTestTask.Data.Repository.Extensions;
using XTestTask.Data.Repository.Interfaces;
using XTestTask.DTO;
using XTestTask.DTO.DChat;
using XTestTask.Middlewares.ExceptionMiddleware.Exceptions;
using XTestTask.Services.Interfaces;

namespace XTestTask.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepositoryManager _rep;
        private readonly IMapper _mapper;

        public ChatService(IRepositoryManager rep, IMapper mapper)
        {
            _rep = rep;
            _mapper = mapper;
        }

        public async Task<List<Chat>> GetChats(Page page)
        {
            return await _rep.Chat
                .FindAll()
                .OrderBy(x => x.Id)
                .TakePage(page)
                .ToListAsync();
        }

        public async Task<Chat?> GetChat(int id)
        {
            return await _rep.Chat
                .FindByCondition(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ChatMember>> GetChatMembers(int chatId, Page page)
        {
            return await _rep.ChatMember
                .FindByCondition(x => x.ChatId == chatId)
                .OrderBy(x => x.AccountId)
                .TakePage(page)
                .ToListAsync();
        }

        public async Task<List<ChatMessage>> GetChatMessages(int chatId, Page page)
        {
            return await _rep.ChatMessage
                .FindByCondition(x => x.ChatId == chatId)
                .OrderByDescending(x => x.CreatedAt)
                .TakePage(page)
                .ToListAsync();
        }

        public async Task<Chat> CreateChat(int userId, ChatDto chatDto)
        {
            if(await _rep.Chat.FindByCondition(x => x.Name == chatDto.Name).AnyAsync())
                throw new BadRequestException("Chat with this name already exists");

            var accountEntity = _rep.Account
                .FindByCondition(x => x.Id == userId)
                .FirstOrDefault() ?? throw new NotFoundException("Account not found");

            var chatEntity = _mapper.Map<Chat>(chatDto);
            chatEntity.CreatorId = userId;
            
            var chatMember = new ChatMember
            {
                AccountId = userId,
                Chat = chatEntity,
            };
            chatEntity.Members.Add(chatMember);

            _rep.Chat.Create(chatEntity);

            await _rep.Save();

            return chatEntity;
        }

        public async Task AddMember(int chatId, int accountId)
        {
            var chatEntity = _rep.Chat
                .FindByCondition(x => x.Id == chatId, true)
                .FirstOrDefault() ?? throw new NotFoundException("Chat not found");

            var accountEntity = _rep.Account
                .FindByCondition(x => x.Id == accountId)
                .FirstOrDefault() ?? throw new NotFoundException("Account not found");

            if(await _rep.ChatMember.FindByCondition(x => x.ChatId == chatId && x.AccountId == accountId).AnyAsync())
                return;

            var chatMember = new ChatMember
            {
                AccountId = accountId,
                ChatId = chatId,
            };

            chatEntity.Members.Add(chatMember);

            await _rep.Save();
        }

        public async Task<ChatMessage> AddMessage(int chatId, int memberId, string message)
        {
            var chatEntity = _rep.Chat
                .FindByCondition(x => x.Id == chatId, true)
                .FirstOrDefault() ?? throw new NotFoundException("Chat not found");

            var memberEntity = _rep.ChatMember
                .FindByCondition(x => x.AccountId == memberId)
                .FirstOrDefault() ?? throw new NotFoundException("Member not found");

            var chatMessage = new ChatMessage
            {
                ChatId = chatId,
                MemberId = memberId,
                Message = message
            };

            chatEntity.Messages.Add(chatMessage);

            await _rep.Save();
            return chatMessage;
        }

        public async Task<Chat> UpdateChat(int chatId, ChatDto chatDto)
        {
            if(await _rep.Chat.FindByCondition(x => x.Name == chatDto.Name && x.Id != chatId).AnyAsync())
                throw new BadRequestException("Chat with this name already exists");

            var chatEntity = _rep.Chat.FindByCondition(x => x.Id == chatId, true)
                .FirstOrDefault() ?? throw new NotFoundException("Chat not found");

            _mapper.Map(chatDto, chatEntity);
            await _rep.Save();

            return chatEntity;
        }

        public async Task<ChatMessage> UpdateMessage(int messageId, MessageDto message)
        {
            var messageEntity = _rep.ChatMessage
                .FindByCondition(x => x.Id == messageId, true)
                .FirstOrDefault() ?? throw new NotFoundException("Message not found");

            _mapper.Map(message, messageEntity);
            await _rep.Save();

            return messageEntity;
        }

        public async Task DeleteChat(int chatId)
        {
            var chatEntity = _rep.Chat
                .FindByCondition(x => x.Id == chatId)
                .FirstOrDefault() ?? throw new NotFoundException("Chat not found");

            _rep.Chat.Delete(chatEntity);
            await _rep.Save();
        }

        public async Task DeleteMessage(int messageId)
        {
            var messageEntity = _rep.ChatMessage
                .FindByCondition(x => x.Id == messageId)
                .FirstOrDefault() ?? throw new NotFoundException("Message not found");

            _rep.ChatMessage.Delete(messageEntity);
            await _rep.Save();
        }

        public async Task DeleteMember(int chatId, int userId)
        {
            var chatMemberEntity = _rep.ChatMember
                .FindByCondition(x => x.ChatId == chatId && x.AccountId == userId)
                .FirstOrDefault() ?? throw new NotFoundException("Member not found");

            _rep.ChatMember.Delete(chatMemberEntity);
            await _rep.Save();
        }
    }
}