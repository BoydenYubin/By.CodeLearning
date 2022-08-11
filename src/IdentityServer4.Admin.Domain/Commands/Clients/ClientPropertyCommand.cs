using ByLearning.Domain.Core.Commands;

namespace ByLearning.Admin.Domain.Commands.Clients
{
    public abstract class ClientPropertyCommand : Command
    {
        public string ClientId { get; protected set; }
        public string Key { get; protected set; }

        public string Value { get; protected set; }
    }
}