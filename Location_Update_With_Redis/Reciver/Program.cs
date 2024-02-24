using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.IService;
using Services.Service;
using System;
internal class Program
{
    private async static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Configuration.AddJsonFile("appSetting.json");
        builder.Services.AddTransient<IRedisSubscribeService> (provider =>new RedisSubscribeService(builder.Configuration["RedisConnectionString"], builder.Configuration["RedisConnectionString"]));
        builder.Services.AddTransient<ISQLiteService>(provider=> new SQLiteService(builder.Configuration["SqlLite"]));
        using IHost host = builder.Build();
        var redisSubscribeService = host.Services.GetRequiredService<IRedisSubscribeService>();
        var sqlLiteUpdateService = host.Services.GetRequiredService<ISQLiteService>();

        // Define a callback to handle the received message
        Action<object> messageReceivedCallback = message =>
        {
            Console.WriteLine($"Received message: {message}");
            sqlLiteUpdateService.UpdateData(message);
        };
        // Start listening with the provided callback
        redisSubscribeService.StartListening(messageReceivedCallback);

        await host.RunAsync();
    }
}