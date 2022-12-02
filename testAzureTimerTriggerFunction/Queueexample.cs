using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure.Storage.Queues;
using Microsoft.Azure.Storage;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace testAzureTimerTriggerFunction
{
    public static class Function1
    {
        public static string responseObj = "Wrong call";

        [FunctionName("Function1")]
        public static void Run([TimerTrigger("*/30 * * * * *")]TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation($"Your are testing");
            AddMessage(responseObj);
            
        }

        

        //Function to add into the queue in the database
        //Used to acces value from loacl.setting.json
        public static string connstring = Environment.GetEnvironmentVariable("connstring");
        public static string queuename = Environment.GetEnvironmentVariable("queue");
        //ConfigurationManager.AppSettings["connstring"];
        public static void AddMessage(string message)
        {

            QueueClient queueClient = new QueueClient(connstring, queuename);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                queueClient.SendMessage(message);
            }
        }

        //get data from the url and see the data in the string
        public static async void senddata(string requestParams)
        {
            // Initialization.  
            

            // HTTP GET.  
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri("your api address");

                // Setting content type.  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                // HTTP GET  
                response = await client.GetAsync("api/WebApi?" + requestParams).ConfigureAwait(false);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    // Reading Response.  
                    string result = response.Content.ReadAsStringAsync().Result;
                    responseObj = JsonConvert.DeserializeObject(result).ToString();
                }
            }
        }

    }
}
