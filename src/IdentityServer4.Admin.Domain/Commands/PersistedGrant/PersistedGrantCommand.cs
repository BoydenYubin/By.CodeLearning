using System;
using System.Collections.Generic;
using System.Text;
using ByLearning.Domain.Core.Commands;

namespace ByLearning.Admin.Domain.Commands.PersistedGrant
{
    public abstract class PersistedGrantCommand : Command
    {
        public string Key { get; protected set; }
    }
}
