﻿using ByLearning.Domain.Core.Commands;

namespace ByLearning.SSO.Domain.Commands.User
{
    public abstract class UserRoleCommand : Command
    {
        public string Username { get; protected set; }
        public string Role { get; protected set; }
    }
}