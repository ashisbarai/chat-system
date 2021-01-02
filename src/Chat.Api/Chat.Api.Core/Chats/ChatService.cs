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
            string sql = string.Join(Environment.NewLine, "INSERT INTO [dbo].[Chats]([Message],[Sender],[Receiver])",
                "VALUES(@Message,@Sender,@Receiver)",
                "SELECT SCOPE_IDENTITY()");
            var id = (await _database.QueryAsync<int>(sql, new
            {
                Message = chat.Message,
                Sender = chat.Sender,
                Receiver = chat.Receiver
            })).First();
            return await GetChatByIdAsync(id);
        }

        public async Task<ChatInfo> GetChatByIdAsync(int id)
        {
            string sql = string.Join(Environment.NewLine, "SELECT [Id],[Message],[Sender],[Receiver],[ReceivedOn]",
                "FROM [dbo].[Chats] WHERE Id=@Id");
            return (await _database.QueryAsync<ChatInfo>(sql, new {Id = id})).FirstOrDefault();
        }
        public async Task<IEnumerable<ChatInfo>> GetUsersChatByUserIdAsync(int userId, int friendId)
        {
            string sql = string.Join(Environment.NewLine, "SELECT [Id],[Message],[Sender],[Receiver],[ReceivedOn]",
                "FROM [dbo].[Chats] WHERE (Sender=@Id AND Receiver=@FriendId) OR (Sender=@FriendId AND Receiver=@Id)",
                "ORDER BY [ReceivedOn]");
            return await _database.QueryAsync<ChatInfo>(sql, new{Id=userId, FriendId=friendId});
        }
    }
}