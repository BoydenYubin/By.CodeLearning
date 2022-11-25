using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ByLearning.SignalR.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/charthub")
                .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                Console.WriteLine("client closing!!");
                await connection.StartAsync();
            };
            connection.On<string, string>("Echo", (user, message) =>
            {
                Console.WriteLine($"user:{user},message:{message}");
            });
            await connection.StartAsync();
            var result = await connection.InvokeAsync<string>("Echo", "Hello", "Message");
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
