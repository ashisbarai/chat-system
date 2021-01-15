using System;
using System.Threading.Tasks;
using Chat.Api.Core.Chats;
using Chat.Api.Core.Messages;
using Chat.Api.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Web.Chats
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ChatService _chatService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hubContext"></param>
        /// <param name="chatService"></param>
        public ChatController(IHubContext<ChatHub> hubContext, ChatService chatService)
        {
            _hubContext = hubContext;
            _chatService = chatService;
        }

        /// <summary>
        /// This API is used for sending a message
        /// </summary>
        /// <remarks>
        /// ```
        /// Request body:
        /// {
        ///     "Sender": "1",
        ///     "Receiver": "2",
        ///     "Message": "message from api"
        /// }
        /// UserId: Sender user id
        /// FriendId: Receiver user id
        /// </remarks>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Route("SendMessage")]
        [HttpPost]
        public async Task<IActionResult> SendMessageAsync([FromBody] MessageDto msg)
        {
            var chat = new ChatInfo
                {UserId = msg.Sender, FriendId = msg.Receiver, Message = msg.Message};

            var senderMessage = await _chatService.CreateChatAsync(chat.ToSenderChatInfo());
            var receiverMessage = await _chatService.CreateChatAsync(chat.ToReceiverChatInfo());

            await _hubContext.Clients.User(msg.Receiver.ToString()).SendAsync("ReceiveOne", receiverMessage);
            await _hubContext.Clients.User(msg.Sender.ToString()).SendAsync("ReceiveOne", senderMessage);

            return Ok();
        }

        /// <summary>
        /// This API gets user's messages for a specific friend.
        /// </summary>
        /// <returns></returns>
        [Route("GetUsersChatByUserId/{userId:int}/{friendId:int}")]
        [HttpGet]
        public async Task<IActionResult> GetUsersChatByUserIdAsync(int userId, int friendId)
        {
            try
            {
                var users = await _chatService.GetUsersChatByUserIdAsync(userId, friendId);
                return Ok(users);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("DeleteUsersChatByUserId/{userId:int}/{friendId:int}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUsersChatByUserIdAsync(int userId, int friendId)
        {
            try
            {
                await _chatService.DeleteUsersChatByUserIdAsync(userId, friendId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("DeleteChatById/{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteChatByIdAsync(int id)
        {
            try
            {
                await _chatService.DeleteChatByIdAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
    }
}
