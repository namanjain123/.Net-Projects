using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Services.IService;
using Services.Service;

namespace Services.Service
{
    public class RedisSubscribeService : IRedisSubscribeService
    {
        private readonly ConnectionMultiplexer connection;
        private readonly string channel;

        public RedisSubscribeService(string redisConnectionString, string channel)
        {
            this.channel = channel;

            connection = ConnectionMultiplexer.Connect(redisConnectionString);
        }
        public void StartListening(Action<object> messageReceivedCallback)
        {

            var pubsub = connection.GetSubscriber();

            pubsub.Subscribe(channel, (receivedChannel, message) =>
            {
                object messageObj = JsonConvert.DeserializeObject<object>(message);
                messageReceivedCallback?.Invoke(messageObj);
            });
        }
    }
}
//// Resolve and use the service
//var redisSubscribeService = serviceProvider.GetRequiredService<IRedisSubscribeService>();

//// Define a callback to handle the received message
//Action<string> messageReceivedCallback = message =>
//{
//    // Handle the received message (replace with your logic)
//    Console.WriteLine($"Received message: {message}");
//};
//// Start listening with the provided callback
//redisSubscribeService.StartListening(messageReceivedCallback);