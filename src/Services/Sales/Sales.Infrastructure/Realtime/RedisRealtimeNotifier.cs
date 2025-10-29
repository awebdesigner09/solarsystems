using Sales.Application.Common.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Sales.Infrastructure.Realtime
{
    public class RedisRealtimeNotifier : IRealtimeNotifier
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisRealtimeNotifier(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task PublishAsync<T>(string channel, T notification, CancellationToken cancellationToken)
        {
            var subscriber = _redis.GetSubscriber();
            var message = JsonSerializer.Serialize(notification);
            await subscriber.PublishAsync(channel, message);
        }
    }
}
