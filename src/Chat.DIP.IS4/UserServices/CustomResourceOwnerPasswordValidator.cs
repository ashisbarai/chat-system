using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Validation;

namespace Chat.DIP.IS4.UserServices
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;

        public CustomResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (await _userRepository.ValidateCredentialsAsync(context.UserName, context.Password))
            {
                var user = await _userRepository.FindByUsernameAsync(context.UserName);
                context.Result = new GrantValidationResult(user.SubjectId, OidcConstants.AuthenticationMethods.Password);
            }

            //return Task.FromResult(0);
        }
    }
}
