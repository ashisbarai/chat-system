using System.Collections.Generic;

namespace Chat.Api.Web.AuthContexts
{
    public class AuthContext
    {
        public List<SimpleClaim> Claims { get; set; }
        public UserProfile UserProfile { get; set; }
    }

    public class UserProfile
    {
        public string Email { get; set; }
    }
}
