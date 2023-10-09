using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using XTestTask.Middlewares.ExceptionMiddleware.Exceptions;

namespace XTestTask.Controllers.Filters
{
    public static class ActionExecutingContextExtensions
    {
        public static string GetValue(this ActionExecutingContext context, string key, string dtoArgumentName)
        {
            var value = "";

            if(context.RouteData.Values.ContainsKey(key))
                value = context.RouteData.Values[key]?.ToString();

            if(string.IsNullOrEmpty(value))
                if(context.HttpContext.Request.Query.ContainsKey(key))
                    value = context.HttpContext.Request.Query[key];
            
            if(string.IsNullOrEmpty(value))
            {
                var body = context.ActionArguments[dtoArgumentName] as dynamic;
                
                if(body != null)
                {
                    var propertyInfo = body.GetType().GetProperty(key);
                    if (propertyInfo != null)
                    {
                        value = propertyInfo.GetValue(body)?.ToString();
                    }
                }
            }

            if(string.IsNullOrEmpty(value))
                throw new BadRequestException($"{key} is required");

            return value;
        }

        private static async Task<string?> ReadValueFromBody(Stream body)
        {
            using (var reader = new StreamReader(body))
            {
                var bodyString = await reader.ReadToEndAsync();
                var reqBody = JsonConvert.DeserializeObject<dynamic>(bodyString);       

                return reqBody?.UserId;
            }
        }
    }
}