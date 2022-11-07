using MassTransit;
using System;

namespace ByLearning.Masstransit.StateMachine.EventModel
{
    public interface IInitialState 
    {
        Guid CorrelationId { get; set; }
    }

    public interface ITrigerA
    {
        Guid CorrelationId { get; set; }
        Guid ProcessingId { get; set; }
    }
    public interface ITrigerB
    {
        Guid CorrelationId { get; set; }
        Guid ProcessingId { get; set; }
    }

    public interface ITrigerC
    {
        Guid CorrelationId { get; set; }
        Guid ProcessingId { get; set; }
    }

    public interface ITrigerD
    {
        Guid CorrelationId { get; set; }
        Guid ProcessingId { get; set; }
    }

    public interface IStateProcessed
    {
        Guid CorrelationId { get; set; }
        Guid ProcessingId { get; set; }
    }
}
