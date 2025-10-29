using Microsoft.AspNetCore.SignalR;
using Sales.API.Hubs;
using StackExchange.Redis;

namespace Sales.API.Services
{
    public class RedisSubscriberService : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly ILogger<RedisSubscriberService> _logger;

        public RedisSubscriberService(IConnectionMultiplexer redis, IHubContext<NotificationsHub> hubContext, ILogger<RedisSubscriberService> logger)
        {
            _redis = redis;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _redis.GetSubscriber();

            // Subscribe to all channels that start with "customer-events:"
            await subscriber.SubscribeAsync("customer-events:*", async (channel, message) =>
            {
                _logger.LogInformation("Received message from Redis channel '{channel}'", channel);

                // Extract customerId from channel name "customer-events:{customerId}"
                var customerId = channel.ToString().Split(':').LastOrDefault();
                if (string.IsNullOrEmpty(customerId)) return;

                try
                {
                    // Forward the message to the specific customer's SignalR group
                    var groupName = NotificationsHub.GetCustomerGroupName(customerId);
                    await _hubContext.Clients.Group(groupName)
                        .SendAsync("QuoteRequestEvent", (string)message, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error forwarding Redis message to SignalR clients.");
                }
            });
        }
    }
}
