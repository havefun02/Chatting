using System.Net.WebSockets;
using System.Text;

namespace App.Services
{
    public static class SockerHandler
    {
        public static async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                // Echo the received message back to the client
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var responseMessage = $"Server: {message}";
                var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, result.EndOfMessage, CancellationToken.None);

                // Wait for the next message
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
