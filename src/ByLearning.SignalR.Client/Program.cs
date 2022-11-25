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
                .WithUrl("http://localhost:5000/intimechathub")
                .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                Console.WriteLine("client closing!!");
                await connection.StartAsync();
            };
            connection.On<string, string>("SendMessage", (user, message) =>
            {
                Console.WriteLine($"user:{user},message:{message}");
            });
            await connection.StartAsync();
            bool first = true;
            string registerUser = string.Empty;
            while (true)
            {
                if (first)
                {
                    Console.WriteLine("Please input the username to chart with each other!");
                    registerUser = Console.ReadLine();
                    if (await connection.InvokeAsync<bool>("RegisterClientInHub", registerUser))
                    {
                        first = false;
                    }
                    continue;
                }
                Console.WriteLine("Please enter the user name to send message");
                var userID = Console.ReadLine();
                if (await connection.InvokeAsync<bool>("SendMessageToClient", userID, $"Message from {registerUser}"))
                    Console.WriteLine("Send sucessfully!");
            }
        }
    }
}
