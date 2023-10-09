using Microsoft.AspNetCore.SignalR;
using XTestTask.Middlewares.ExceptionMiddleware.Exceptions;

namespace XTestTask.Hubs.Filters
{
    public class ExceptionFilter : IHubFilter
    {
        public async ValueTask<object> InvokeMethodAsync(
            HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            try
            {
                return await next(invocationContext);
            }
            catch (Exception ex)
            {
                return ex.Message ?? "Something went wrong";
            }
        }
    }
}