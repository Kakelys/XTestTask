using Microsoft.EntityFrameworkCore;
using XTestTask.Data.Repository.Implements;
using XTestTask.Data.Repository.Interfaces;

namespace XTestTask.Data.Repository
{
    public static class RepositoryService
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options => 
                options.UseNpgsql(config.GetConnectionString("XTestTask")));

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatMemberRepository, ChatMemberRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            return services;
        }
    }
}