using Microsoft.Extensions.DependencyInjection;

namespace Chat.DIP.IS4.UserServices
{
    public static class CustomIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.AddProfileService<CustomUserProfileService>();
            builder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();

            return builder;
        }
    }
}