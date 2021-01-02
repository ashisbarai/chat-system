using System;

namespace Chat.Api.Core.Chats
{
    public class ChatInfo
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public DateTime ReceivedOn { get; set; }
    }
}