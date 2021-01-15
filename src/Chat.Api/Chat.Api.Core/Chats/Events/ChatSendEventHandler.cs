using System.Threading.Tasks;
using Chat.Api.Core.Events;

namespace Chat.Api.Core.Chats.Events
{
    public class ChatSendEventHandler : IEventHandler<ChatSendEvent>
    {
        private readonly ChatService _chatService;

        public ChatSendEventHandler(ChatService chatService)
        {
            _chatService = chatService;
        }
        public async Task RunAsync(ChatSendEvent obj)
        {
            await _chatService.CreateChatAsync(obj.ChatInfo.ToSenderChatInfo());
            await _chatService.CreateChatAsync(obj.ChatInfo.ToReceiverChatInfo());
        }
    }
}
