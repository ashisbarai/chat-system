using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Api.Core.Chats;
using Chat.Api.Core.Chats.Events;
using Chat.Api.Core.Events;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly EventService _eventService;
        public static Dictionary<int, string> ClientMapper = new Dictionary<int, string>();

        public ChatHub(EventService eventService)
        {
            _eventService = eventService;
        }
        public async Task Send(int sender, int receiver, string message)
        {
            string connectionId;
            ClientMapper.TryGetValue(receiver, out connectionId);
            string senderConnectionId;
            ClientMapper.TryGetValue(sender, out senderConnectionId);
            //return Clients.All.SendAsync("ReceiveOne", user, message);
            await _eventService.PublishAsync(new ChatSendEvent(new ChatInfo
                {Sender = sender, Receiver = receiver, Message = message}));

            await Clients.Clients(connectionId ?? "", senderConnectionId ?? "").SendAsync("ReceiveOne", receiver, sender, message);
        }
        public string GetConnectionId(int id)
        {
            if (ClientMapper.ContainsKey(id))
            {
                ClientMapper[id] = Context.ConnectionId;
            }
            else
            {
                ClientMapper.Add(id, Context.ConnectionId);
            }
            return Context.ConnectionId;
        }
    }
}
