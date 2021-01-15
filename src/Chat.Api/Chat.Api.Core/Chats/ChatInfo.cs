using System;

namespace Chat.Api.Core.Chats
{
    public class ChatInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public static class ChatInfoExtension
    {
        public static ChatInfo ToSenderChatInfo(this ChatInfo chatInfo)
        {
            return new ChatInfo
            {
                UserId = chatInfo.UserId,
                FriendId = chatInfo.FriendId,
                Message = chatInfo.Message,
                MessageType = MessageType.Send
            };
        }
        public static ChatInfo ToReceiverChatInfo(this ChatInfo chatInfo)
        {
            return new ChatInfo
            {
                UserId = chatInfo.FriendId,
                FriendId = chatInfo.UserId,
                Message = chatInfo.Message,
                MessageType = MessageType.Received
            };
        }
    }
}