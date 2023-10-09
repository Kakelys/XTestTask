using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace XTestTaskTests.IntegrationTests
{
    public static class HttpContentHelper
    {
        public static ByteArrayContent Convert(object obj)
        {
            var content = JsonConvert.SerializeObject(obj);

            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }
    }
}