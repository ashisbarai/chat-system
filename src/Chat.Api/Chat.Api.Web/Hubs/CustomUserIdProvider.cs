using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Web.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var id = connection.User?.FindFirst("sub")?.Value;
            return connection.User?.FindFirst("sub")?.Value;
        }
    }
}
