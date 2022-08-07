using ByLearning.Domain.Core.Commands;

namespace ByLearning.SSO.Domain.Commands.GlobalConfiguration
{
    public abstract class GlobalConfigurationCommand : Command
    {
        public string Key { get; protected set; }
        public string Value { get; protected set; }
        public bool Sensitive { get; protected set; }
        public bool IsPublic { get; protected set; }
        public bool Public { get; protected set; }
    }
}
