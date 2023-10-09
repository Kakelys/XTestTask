using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace XTestTaskTests.IntegrationTests
{
    public class BaseIntegrationTests
    {
        protected HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown() 
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}