
namespace XTestTask.Middlewares.ExceptionMiddleware.Exceptions
{
    public class ForbiddenException : ExceptionBase
    {
        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}