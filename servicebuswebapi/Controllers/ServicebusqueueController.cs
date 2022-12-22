using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Identity;

namespace servicebuswebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicebusqueueController : ControllerBase
    {
        [HttpPost]
        public async Task<string> senddatatoqueue([FromBody] string message)
        {
            ServiceBusClient client;
            ServiceBusSender sender;
            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
           // Replace the "<NAMESPACE-NAME>" and "<QUEUE-NAME>" placeholders.
        client = new ServiceBusClient(
                        "<NAMESPACE-NAME>.servicebus.windows.net",
                        new DefaultAzureCredential(),
                    clientOptions);
            sender = client.CreateSender("<QUEUE-NAME>");
            //create message batch
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            //addition of message in the message batch
            messageBatch.TryAddMessage(new ServiceBusMessage(message));
            // We are using messages but if you want only one then can easily just use the direct sting and chnage to message
            await sender.SendMessagesAsync(messageBatch);
            //Dispose the send er and the client may be commented later
            await sender.DisposeAsync();
            await client.DisposeAsync();
            return "message send";
        }
    }
}
