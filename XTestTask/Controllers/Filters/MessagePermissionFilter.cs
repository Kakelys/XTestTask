using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using XTestTask.Data.Repository.Interfaces;
using XTestTask.Middlewares.ExceptionMiddleware.Exceptions;

namespace XTestTask.Controllers.Filters
{
    public class MessagePermissionFilter : ActionFilterAttribute
    {
        private string _dtoArgumentName;
        private string _messageIdKey = "messageId";
        private string _userIdKey = "userId";

        public MessagePermissionFilter(string dtoArgumentName)
        {
            _dtoArgumentName = dtoArgumentName;
        }

        public MessagePermissionFilter(string dtoArgumentName, string messageIdKey) : this(dtoArgumentName)
        {
            _messageIdKey = messageIdKey;
        }

        public MessagePermissionFilter(string dtoArgumentName, string messageIdKey, string userIdKey) : this(dtoArgumentName, messageIdKey)
        {
            _userIdKey = userIdKey;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!int.TryParse(context.GetValue(_messageIdKey, _dtoArgumentName), out var messageId))
                throw new BadRequestException("invalid messageId");

            if(!int.TryParse(context.GetValue(_userIdKey, _dtoArgumentName), out var userId))
                throw new BadRequestException("invalid userId");

            var rep = context.HttpContext.RequestServices.GetService<IRepositoryManager>()
                ?? throw new Exception("Cannot get repository manager");

            if(!await rep.ChatMessage.FindByCondition(c => c.Id == messageId && c.MemberId == userId).AnyAsync())
                throw new ForbiddenException("You are not created this message and cannot perform this action");

            await next();
        }
    }
}