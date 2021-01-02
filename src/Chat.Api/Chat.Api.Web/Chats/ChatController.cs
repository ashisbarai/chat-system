using System;
using System.Threading.Tasks;
using Chat.Api.Core.Chats;
using Chat.Api.Core.Messages;
using Chat.Api.Web.Hubs;
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
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Route("SendMessage")]                                           //path looks like this: https://localhost:44379/api/chat/send
        [HttpPost]
        public IActionResult SendRequest([FromBody] MessageDto msg)
        {
            _hubContext.Clients.User(msg.User).SendAsync("ReceiveOne", msg.User, msg.MsgText);
            return Ok();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [Route("CreateChat")]
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] ChatInfo chat)
        {
            try
            {
                var newChat = await _chatService.CreateChatAsync(chat);
                return Ok(newChat);
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
    }
}
