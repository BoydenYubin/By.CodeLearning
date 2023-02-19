using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ByLerning.SignalR
{
    public interface IChatClient
    {
        Task SendMessage(string user, string message);
    }

    public interface ITestInjection { }
    public class TestInjection : ITestInjection { }

    public class StronglyTypedChatHub : Hub<IChatClient>
    {
        private static ConcurrentDictionary<string, string> _clientLists = new ConcurrentDictionary<string, string>();
        private readonly ITestInjection injection;

        public StronglyTypedChatHub(ITestInjection injection)
        {
            this.injection = injection;
        }
        public Task<bool> RegisterClientInHub(string userID)
        {
            try
            {
                RegisterClientId(userID);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Clients.Caller.SendMessage(Context.UserIdentifier, $"Error Message: {e.Message}");
                return Task.FromResult(false);
            }
        }

        private string RegisterClientId(string userID)
        {
            var connectionId = _clientLists.AddOrUpdate(userID, Context.ConnectionId, (key, value) =>
            {
                return Context.ConnectionId;
            });
            Console.WriteLine($"Registed User {userID}");
            return connectionId;
        }

        public Task<bool> SendMessageToClient(string receiveUserId, string message)
        {
            try
            {
                Console.WriteLine(injection.GetHashCode());
                if (_clientLists.TryGetValue(receiveUserId, out string userID))
                {
                    Clients.Client(userID).SendMessage(Context.ConnectionId, message);
                }
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Clients.Caller.SendMessage(Context.ConnectionId, $"Error Message: {e.Message}");
                return Task.FromResult(false);
            }
        }
    }
}
