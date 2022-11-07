using ByLearning.Masstransit.StateMachine.EventModel;
using MassTransit;
using System;

namespace ByLearning.Masstransit.StateMachine.StateMachineModel
{
    public class ByLearningStateMachine : MassTransitStateMachine<ByLearningState>
    {
        public ByLearningStateMachine()
        {
            InstanceState(state => state.State);

            //A composite event is configured by specifying one or more events that must be consumed, after which the composite event will be raised. 
            //http://masstransit-project.com/usage/sagas/automatonymous.html#composite-event
            CompositeEvent(() => CompositeEventCD, x => x.ReadyEventStatus, TrigerCEvent, TrigerDEvent);

            Initially(
                When(StateInitialed)
                .Then(StateInitialLogContext)
                .TransitionTo(StateInitial));

            During(StateInitial,
                When(TrigerAEvent)
                .Then(context => Console.WriteLine($"From StateInitial to TriggedByAorB: "))
                .TransitionTo(TriggedByAorB),
                When(TrigerBEvent)
                .Then(context => Console.WriteLine($"From StateInitial to TriggedByAorB: "))
                .TransitionTo(TriggedByAorB));

            During(TriggedByAorB,
                When(CompositeEventCD)
                .Then(context => Console.WriteLine($"From TriggedByAorB to TriggedByAadnB: "))
                .TransitionTo(TriggedByAadnB));

            During(TriggedByAadnB,
                When(StateProcessed)
                .Then(context => Console.WriteLine($"From TriggedByAadnB to StateCompleted: "))
                .TransitionTo(StateCompleted).Finalize());
        }
        /// <summary>
        /// If use <see cref="State"/> in <see cref="ByLearningState"/>, 
        /// StateInitial, TriggedByAorB, TriggedByAadnB, StateCompleted can be inserted without configuration
        /// If <see cref="string"/> <see cref="int"/> was used in  <see cref="ByLearningState"/>, 
        /// we should use the InstanceState(x => x.State, StateInitial, TriggedByAorB, TriggedByAadnB, StateCompleted) instead,
        /// </summary>
        public State StateInitial { get; private set; }
        public State TriggedByAorB { get; private set; }
        public State TriggedByAadnB { get; private set; }
        public State StateCompleted { get; private set; }

        /// <summary>
        /// Triger Event
        /// </summary>
        public Event<IInitialState> StateInitialed { get; private set; }
        public Event<ITrigerA> TrigerAEvent { get; private set; }
        public Event<ITrigerB> TrigerBEvent { get; private set; }
        public Event CompositeEventCD { get; private set; }
        public Event<ITrigerC> TrigerCEvent { get; private set; }
        public Event<ITrigerD> TrigerDEvent { get; private set; }
        public Event<IStateProcessed> StateProcessed { get; private set; }

        private void StateInitialLogContext(BehaviorContext<ByLearningState,IInitialState> context)
        {
            Console.WriteLine($"StateInitialed event triger to StateInitial: {context.Message.CorrelationId}");
        }
    }
}
