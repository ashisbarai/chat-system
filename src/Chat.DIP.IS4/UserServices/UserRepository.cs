using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.DIP.IS4.Data;

namespace Chat.DIP.IS4.UserServices
{
    public class UserRepository : IUserRepository
    {
        private readonly Database _database;

        public UserRepository(Database database)
        {
            _database = database;
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            try
            {
                var user = (await _database.QueryAsync<int>("SELECT Id FROM Users WHERE Email=@Email", new {Email = email})).ToList();
                return user.Any();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CustomUser> FindBySubjectIdAsync(string subjectId)
        {
            var user = (await _database.QueryAsync<CustomUser>("SELECT Id,Email,FirstName,LastName FROM Users WHERE Id=@Id",
                new {Id = subjectId})).FirstOrDefault();

            return user;
        }

        public async Task<CustomUser> FindByUsernameAsync(string username)
        {
            var user = (await _database.QueryAsync<CustomUser>("SELECT Id,Email,FirstName,LastName FROM Users WHERE Email=@Email",
                new { Email = username })).FirstOrDefault();

            return user;
        }
    }
}
