using System.Threading.Tasks;

namespace Chat.DIP.IS4.UserServices
{
    public interface IUserRepository
    {
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task<CustomUser> FindBySubjectIdAsync(string subjectId);
        Task<CustomUser> FindByUsernameAsync(string username);
    }
}
