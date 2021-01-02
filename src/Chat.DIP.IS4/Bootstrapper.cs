using Chat.DIP.IS4.Configs;
using Chat.DIP.IS4.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.DIP.IS4
{
    public class Bootstrapper
    {
        public static void Build(IServiceCollection services, IConfiguration configuration)
        {
            BuildDatabaseConfig(services, configuration);

            services.AddTransient<Database>();

            //services.AddScoped<AuthHttpClient>();

        }
        private static void BuildDatabaseConfig(IServiceCollection services, IConfiguration configuration)
        {
            var databaseConfig =
                configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>();
            services.AddSingleton(databaseConfig);
        }
    }
}