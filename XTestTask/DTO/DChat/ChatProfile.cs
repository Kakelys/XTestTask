using AutoMapper;
using XTestTask.Data.Models;

namespace XTestTask.DTO.DChat
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<ChatDto, Chat>();
            CreateMap<MessageDto, ChatMessage>();
        }        
    }
}