using Chat.Api.Core.Chats;
using Chat.Api.Core.Chats.Events;
using Chat.Api.Core.Configs;
using Chat.Api.Core.DatabaseTests;
using Chat.Api.Core.Events;
using Chat.Api.Core.Interfaces;
using Chat.Api.Core.Users;
using Chat.Api.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Api.Web
{
    public class Bootstrapper
    {
        public static void Build(IServiceCollection services, IConfiguration configuration)
        {
            BuildDatabaseConfig(services, configuration);
            //RegisterDatabaseFactory(services);

            services.AddTransient<IDatabase, Database>();
            BuildApiKeyConfig(services, configuration);

            //RegisterEvents(services);

            services.AddScoped<DatabaseTestService>();
            services.AddScoped<UserService>();
            services.AddScoped<ChatService>();
        }

        private static void RegisterEvents(IServiceCollection services)
        {
            services.AddScoped<EventService>();


            services.AddScoped<ChatSendEventHandler>();
            EventService.Subscribe<ChatSendEvent, ChatSendEventHandler>();
        }
        private static void BuildDatabaseConfig(IServiceCollection services, IConfiguration configuration)
        {
            var databaseConfig =
                configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>();
            services.AddSingleton(databaseConfig);
        }
        private static void BuildApiKeyConfig(IServiceCollection services, IConfiguration configuration)
        {
            var databaseConfig =
                configuration.GetSection("ApiKeyConfig").Get<ApiKeyConfig>();
            services.AddSingleton(databaseConfig);
        }
    }
}