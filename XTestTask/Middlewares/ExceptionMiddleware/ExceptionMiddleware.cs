using XTestTask.Middlewares.ExceptionMiddleware.Exceptions;

namespace XTestTask.Middlewares.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(BadRequestException ex)
            {
                await SendExceptionResponse(context, ex, 400, ex.Message ?? "Bad request");
            }
            catch(ForbiddenException ex)
            {
                await SendExceptionResponse(context, ex, 403, ex.Message ?? "Forbidden");
            }
            catch(NotFoundException ex)
            {
                await SendExceptionResponse(context, ex, 404, ex.Message ?? "Not found");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await SendExceptionResponse(context, ex, 500, "Internal server error");
            }
        }

        private async Task SendExceptionResponse(HttpContext context, Exception ex, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(message);
        }
    }
}