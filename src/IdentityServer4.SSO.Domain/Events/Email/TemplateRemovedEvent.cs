using ByLearning.Domain.Core.Events;
using System;

namespace ByLearning.SSO.Domain.Events.Email
{
    public class TemplateRemovedEvent : Event
    {
        public TemplateRemovedEvent(Guid templateId)
        {
            AggregateId = templateId.ToString();
            Message = "Template";
        }
    }
}