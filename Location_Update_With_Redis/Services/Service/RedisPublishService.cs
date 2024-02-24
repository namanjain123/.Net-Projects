using StackExchange.Redis;
using Newtonsoft.Json;
using Services.IService;

namespace Services.Service
{
    public class RedisPublishService : IRedisPublishService
    {
        private string RedisConnectionString;
        private static ConnectionMultiplexer connection;
        private string Channel;
        public RedisPublishService(string connectionString, string channel)
        {
            RedisConnectionString = connectionString;
            Channel = channel;
            connection = ConnectionMultiplexer.Connect(RedisConnectionString);
        }
        public async Task<string> PublishMessageAsync(object message)
        {
            var pubsub = connection.GetSubscriber();
            await pubsub.PublishAsync(Channel, JsonConvert.SerializeObject(message), CommandFlags.FireAndForget);
            return $"Message Successfully sent to {Channel}";
        }

    }

}
