using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Api.Core.Interfaces;

namespace Chat.Api.Core.Chats
{
    public class ChatService
    {
        private readonly IDatabase _database;

        public ChatService(IDatabase database)
        {
            _database = database;
        }

        public async Task<ChatInfo> CreateChatAsync(ChatInfo chat)
        {
            string sql = string.Join(Environment.NewLine, "INSERT INTO [dbo].[Chats]([Message],[UserId],[FriendId],[MessageType])",
                "VALUES(@Message,@UserId,@FriendId,@MessageType)",
                "SELECT SCOPE_IDENTITY()");
            var id = (await _database.QueryAsync<int>(sql, new
            {
                Message = chat.Message,
                UserId = chat.UserId,
                FriendId = chat.FriendId,
                MessageType = chat.MessageType
            })).First();
            return await GetChatByIdAsync(id);
        }

        public async Task<ChatInfo> GetChatByIdAsync(int id)
        {
            string sql = string.Join(Environment.NewLine, "SELECT [Id],[Message],[UserId],[FriendId],[MessageType],[CreatedOn]",
                "FROM [dbo].[Chats] WHERE Id=@Id");
            return (await _database.QueryAsync<ChatInfo>(sql, new {Id = id})).FirstOrDefault();
        }

        public async Task<IEnumerable<ChatInfo>> GetUsersChatByUserIdAsync(int userId, int friendId)
        {
            string sql = string.Join(Environment.NewLine,
                "SELECT [Id],[Message],[UserId],[FriendId],[MessageType],[CreatedOn]",
                "FROM [dbo].[Chats] WHERE UserId=@UserId AND FriendId=@FriendId",
                "ORDER BY [CreatedOn]");
            return await _database.QueryAsync<ChatInfo>(sql, new {UserId = userId, FriendId = friendId});
        }

        public async Task DeleteUsersChatByUserIdAsync(int userId, int friendId)
        {
            string sql = string.Join(Environment.NewLine, "DELETE FROM [dbo].[Chats]",
                "WHERE UserId=@UserId AND FriendId=@FriendId");
             await _database.ExecuteAsync(sql, new { UserId = userId, FriendId = friendId });
        }

        public async Task DeleteChatByIdAsync(int id)
        {
            await _database.ExecuteAsync("DELETE [dbo].[Chats]  WHERE Id=@Id", new {Id = id});
        }
    }
}