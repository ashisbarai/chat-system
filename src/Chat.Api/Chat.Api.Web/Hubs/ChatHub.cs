using System;
using System.Threading.Tasks;
using Chat.Api.Core.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }
        public async Task Send(int sender, int receiver, string message)
        {
            try
            {
                var chat = new ChatInfo
                    {UserId = sender, FriendId = receiver, Message = message};

                var senderMessage = await _chatService.CreateChatAsync(chat.ToSenderChatInfo());
                var receiverMessage = await _chatService.CreateChatAsync(chat.ToReceiverChatInfo());

                await Clients.User(receiver.ToString()).SendAsync("ReceiveOne", receiverMessage);
                await Clients.User(sender.ToString()).SendAsync("ReceiveOne", senderMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}