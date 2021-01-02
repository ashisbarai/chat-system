using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Chat.DIP.IS4.Quickstart.Account
{
    public class CustomProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = TestUsers.Users.First(u=>u.SubjectId == sub);

            var claims = context.IssuedClaims;

            if (user.Username == "bob")
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "Admin"));
            }
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = TestUsers.Users.FirstOrDefault(u=>u.SubjectId == sub);
            context.IsActive = user != null;
        }
    }
}
