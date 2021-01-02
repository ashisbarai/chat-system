using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace Chat.DIP.IS4.UserServices
{
    public class CustomUserProfileService : IProfileService
    {
        protected readonly ILogger Logger;


        protected readonly IUserRepository UserRepository;

        public CustomUserProfileService(IUserRepository userRepository, ILogger<CustomUserProfileService> logger)
        {
            UserRepository = userRepository;
            Logger = logger;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            Logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
                context.Subject.GetSubjectId(),
                context.Client.ClientName ?? context.Client.ClientId,
                context.RequestedClaimTypes,
                context.Caller);

            var user = await UserRepository.FindBySubjectIdAsync(sub);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, sub),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.GivenName, user.FirstName ?? ""),
                new Claim(JwtClaimTypes.FamilyName, user.LastName ?? "")
            };

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await UserRepository.FindBySubjectIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}