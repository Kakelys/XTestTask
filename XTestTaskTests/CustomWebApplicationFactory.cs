using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using XTestTask.Data;

namespace XTestTaskTests
{
    public class CustomWebApplicationFactory<T> : WebApplicationFactory<T> where T: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => 
                {
                    //rewrite existing dbcontext registration to test db
                    var existingRegistration = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (existingRegistration != null)
                    {
                        services.Remove(existingRegistration);
                    }

                    services.AddDbContext<AppDbContext>(options => 
                        options.UseNpgsql("Host=localhost;Port=5432;Database=XTestTaskTests;Username=postgres;Password=.Qwerty1%;"));
                });

            base.ConfigureWebHost(builder);
        }
    }
}