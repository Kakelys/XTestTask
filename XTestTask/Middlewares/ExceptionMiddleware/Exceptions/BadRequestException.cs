
namespace XTestTask.Middlewares.ExceptionMiddleware.Exceptions
{
    public class BadRequestException : ExceptionBase
    {
        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}