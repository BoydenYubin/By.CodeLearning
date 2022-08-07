using ByLearning.Domain.Core.Events;
using ByLearning.SSO.Domain.Models;

namespace ByLearning.SSO.Domain.Events.Email
{
    public class TemplateAddedEvent : Event
    {
        public Template Template { get; }

        public TemplateAddedEvent(Template template)
            : base(EventTypes.Success)
        {
            Template = template;
            AggregateId = template.Id.ToString();
            Message = "Template Added";
        }
    }
}
