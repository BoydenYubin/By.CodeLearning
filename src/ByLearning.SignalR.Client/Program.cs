using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ByLearning.SignalR.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/intimechathub")
                .WithUrl("http://localhost:5000/charthub")
                .WithUrl("http://localhost:5000/streamhub")
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

            var channel = await connection.StreamAsChannelAsync<byte[]>("DownloadFileTest");
            var file = new FileStream("test.zip", FileMode.CreateNew);
            while (await channel.WaitToReadAsync())
            {
                // Read all currently available data synchronously, before waiting for more data
                while (channel.TryRead(out var buffer))
                {
                    file.Write(buffer, 0, buffer.Length);
                }
            }
            file.Flush();
            file.Close();

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
