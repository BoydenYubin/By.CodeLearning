using Microsoft.AspNetCore.SignalR;

namespace ByLerning.SignalR
{
    public class ChartHub : Hub
    {
        public string Echo(string user,string message)
        {
            System.Console.WriteLine($"user:{user},message:{message}");
            return $"replyed message from server: {message}";
        }
    }
}
