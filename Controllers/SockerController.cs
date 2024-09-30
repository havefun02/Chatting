using App.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using System.Net.WebSockets;

namespace App.Controllers
{
    [ApiController]

    [Route("/ws")]
    public class SockerController:ControllerBase
    {
        [HttpGet]
        public async Task Get()
        {
            Log.Information("Socket connected");
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                Log.Information("User connected");
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await SockerHandler.HandleWebSocketAsync(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400; // Bad Request
            }
        }
    }
}
