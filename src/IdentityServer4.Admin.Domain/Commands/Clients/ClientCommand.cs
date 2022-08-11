using IdentityServer4.Models;
using ByLearning.Domain.Core.Commands;

namespace ByLearning.Admin.Domain.Commands.Clients
{
    public abstract class ClientCommand : Command
    {
        public Client Client { get; set; }
        public string OldClientId { get; protected set; }
    }
}