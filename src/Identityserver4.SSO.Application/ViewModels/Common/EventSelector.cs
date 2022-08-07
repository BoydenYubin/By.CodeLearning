using ByLearning.Domain.Core.Util;

namespace Identityserver4.SSO.Application.ViewModels.Common
{
    public class EventSelector
    {
        public EventSelector() { }
        public EventSelector(AggregateType type, string aggregate)
        {
            AggregateType = type.ToString().AddSpacesToSentence();
            Aggregate = aggregate;
        }

        public string AggregateType { get; set; }
        public string Aggregate { get; set; }
    }
}
