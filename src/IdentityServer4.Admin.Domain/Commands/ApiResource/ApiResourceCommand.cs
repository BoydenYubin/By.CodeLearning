using ByLearning.Domain.Core.Commands;

namespace ByLearning.Admin.Domain.Commands.ApiResource
{
    public abstract class ApiResourceCommand : Command
    {
        public string OldResourceName { get; protected set; }
        public IdentityServer4.Models.ApiResource Resource { get; protected set; }


    }
}