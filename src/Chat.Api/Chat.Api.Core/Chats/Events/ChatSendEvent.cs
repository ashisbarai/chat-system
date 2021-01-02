using Chat.Api.Core.Events;

namespace Chat.Api.Core.Chats.Events
{
    public class ChatSendEvent : EventBase
    {
        public ChatSendEvent(ChatInfo chatInfo)
        {
            ChatInfo = chatInfo;
        }

        public ChatInfo ChatInfo { get; set; }
    }
}
