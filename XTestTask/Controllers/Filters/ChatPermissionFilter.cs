using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using XTestTask.Data.Repository.Interfaces;
using XTestTask.Middlewares.ExceptionMiddleware.Exceptions;

namespace XTestTask.Controllers.Filters
{
    public class ChatPermissionFilter : ActionFilterAttribute
    {
        private string _dtoArgumentName;
        private string _chatIdKey = "chatId";
        private string _userIdKey = "userId";

        public ChatPermissionFilter(string dtoArgumentName)
        {
            _dtoArgumentName = dtoArgumentName;
        }

        public ChatPermissionFilter(string dtoArgumentName, string chatIdKey) : this(dtoArgumentName)
        {
            _chatIdKey = chatIdKey;
        }

        public ChatPermissionFilter(string dtoArgumentName, string chatIdKey, string userIdKey) : this(dtoArgumentName, chatIdKey)
        {
            _userIdKey = userIdKey;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!int.TryParse(context.GetValue(_userIdKey, _dtoArgumentName), out var userId))
                throw new BadRequestException("invalid userId");

            if(!int.TryParse(context.GetValue(_chatIdKey, _dtoArgumentName), out var chatId))
                throw new BadRequestException("invalid chatId");

            var rep = context.HttpContext.RequestServices.GetService<IRepositoryManager>()
                ?? throw new Exception("Cannot get repository manager");

            if(!await rep.Chat.FindByCondition(c => c.Id == chatId && c.CreatorId == userId).AnyAsync())
                throw new ForbiddenException("You are not creator of this chat to perform this action");

            await next();
        }
    }
}