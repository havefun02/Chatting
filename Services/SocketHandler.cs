using App.Core;
using App.Models;
using System.Net.WebSockets;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace App.Services
{
    public  class SocketHandler
    {
        private readonly SocketManager _subscriberManager;
        private readonly IAppService _appService;
        public SocketHandler(IAppService appService, SocketManager manager)
        {
            _appService = appService; 
            _subscriberManager=manager;
        }
        public void CloseAllConnections()
        {
          
        }
        public async Task HandleWebSocketAsync(WebSocket webSocket,string UserId)
        {
            _subscriberManager.AddSubscriber(UserId, webSocket);
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var messageResult=await this._appService.AddMessage(new MessageDto {MessageId=Guid.NewGuid().ToString() ,UserId=UserId,Content=message});
                await _subscriberManager.NotifySubscribers(messageResult); // Notify all users
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            _subscriberManager.RemoveSubscriber(UserId);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            webSocket.Dispose();
        }
    }
}
