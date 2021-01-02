using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Api.Core.Interfaces;

namespace Chat.Api.Core.Users
{
    public class UserService
    {
        private readonly IDatabase _database;

        public UserService(IDatabase database)
        {
            _database = database;
        }

        public async Task<UserInfoDto> CreateUserAsync(UserInfo user)
        {
            string sql = string.Join(Environment.NewLine, "INSERT INTO [dbo].[Users]([Email],[FirstName],[LastName])",
                "VALUES(@Email,@FirstName,@LastName)",
                "SELECT SCOPE_IDENTITY()");
            var id = (await _database.QueryAsync<int>(sql, new
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            })).First();
            return await GetUserByIdAsync(id);
        }

        public async Task<UserInfoDto> GetUserByIdAsync(int id)
        {
            string sql = string.Join(Environment.NewLine, "SELECT [Id],[Email],[FirstName],[LastName],[CreatedOn]",
                "FROM [dbo].[Users] WHERE Id=@Id");
            return (await _database.QueryAsync<UserInfoDto>(sql, new {Id = id})).FirstOrDefault();
        }
        public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync()
        {
            string sql = string.Join(Environment.NewLine, "SELECT [Id],[Email],[FirstName],[LastName],[CreatedOn]",
                "FROM [dbo].[Users]",
                "ORDER BY [FirstName],[LastName]");
            return await _database.QueryAsync<UserInfoDto>(sql);
        }
        public async Task<IEnumerable<UserInfoDto>> GetAllFriendsByUserIdAsync(int id)
        {
            string sql = string.Join(Environment.NewLine, "SELECT [Id],[Email],[FirstName],[LastName],[CreatedOn]",
                "FROM [dbo].[Users] WHERE Id!=@Id",
                "ORDER BY [FirstName],[LastName]");
            return await _database.QueryAsync<UserInfoDto>(sql, new{Id=id});
        }
    }
}