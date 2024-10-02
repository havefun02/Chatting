using App.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using System.Net.WebSockets;

namespace App.Controllers
{
    [ApiController]
    [Route("/ws")]
    public class SocketController:ControllerBase
    {
        private readonly SocketHandler _sockerHandler;   

        public SocketController(SocketHandler sockerHandler) {
            _sockerHandler = sockerHandler;
        }
        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var userId=string.Empty;
                try
                {
                    if (HttpContext.Request.Cookies.TryGetValue("UserId", out userId))
                    {
                        await _sockerHandler.HandleWebSocketAsync(webSocket, userId);
                    }
                    else
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.Empty, "Fail to connect", CancellationToken.None);
                         webSocket.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error handling WebSocket connection: {Message}", ex.Message);
                    await webSocket.CloseAsync(WebSocketCloseStatus.Empty, "Internal Sever Error", CancellationToken.None);
                    webSocket.Dispose();
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 400; // Bad Request
            }
        }
    }
}
