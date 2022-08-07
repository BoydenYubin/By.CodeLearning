﻿using MediatR;
using System;

namespace ByLearning.Domain.Core.Events
{
    public abstract class Message : IRequest
    {
        public string MessageType { get; protected set; }
        public string AggregateId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
