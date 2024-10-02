using App.Models;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace App.Services
{
    public class SocketManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _subscribers = new();

        public void AddSubscriber(string userId, WebSocket webSocket)
        {
            _subscribers[userId] = webSocket;
        }

        public void RemoveSubscriber(string userId)
        {
            _subscribers.TryRemove(userId, out _);
        }

        public WebSocket? GetSubscriber(string userId)
        {
            _subscribers.TryGetValue(userId, out var webSocket);
            return webSocket; // Return null if not found
        }

        public IEnumerable<string> GetAllSubscribers()
        {
            return _subscribers.Keys; // Get all user IDs of the subscribers
        }

        public async Task NotifySubscribers(Message data, CancellationToken cancellationToken = default)
        {
            var formattedMessage = new
            {
                UserName = data.User?.UserName,
                Content = data.Content,
                CreatedAt = data.CreatedAt.ToString("MM/dd/yyyy hh:mm:ss tt")
            };
            string jsonMessage = JsonSerializer.Serialize(formattedMessage);
            var responseBuffer = Encoding.UTF8.GetBytes(jsonMessage);

            var tasks = _subscribers
                .Where(subscriber => subscriber.Value.State == WebSocketState.Open) // Only send to open connections
                .Select(subscriber =>
                    subscriber.Value.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, cancellationToken)
                    .ContinueWith(t => {
                        if (t.IsFaulted)
                        {
                            RemoveSubscriber(subscriber.Key);
                        }
                    }, cancellationToken)
                );

            await Task.WhenAll(tasks); // Await all send tasks
        }
        public async Task ClearAllConnections(CancellationToken cancellationToken = default)
        {
            foreach (var subscriber in _subscribers)
            {
                if (subscriber.Value.State == WebSocketState.Open)
                {
                    await subscriber.Value.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application is stopping", cancellationToken);
                    RemoveSubscriber(subscriber.Key); 
                }
            }
        }
    }

}
