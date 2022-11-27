﻿using MassTransit;
using System;

namespace ByLearning.Masstransit.StateMachine.StateMachineModel
{
    public class ByLearningState : SagaStateMachineInstance, ISagaVersion
    {
        protected ByLearningState() { }
        public ByLearningState(Guid correlationId)
        {
            this.CorrelationId = correlationId;
        }
        /// <summary>
        /// The state of the saga
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// When the routing slip was started
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// When the routing slip was completed
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// The total duration of the routing slip
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// When the routing slip was created
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// The fault summary is an exception summary for the faulted routing slip
        /// </summary>
        public string FaultSummary { get; set; }

        /// <summary>
        /// This maps to the tracking number of the routing slip
        /// </summary>
        public Guid CorrelationId { get; set; }
        public int ReadyEventStatus { get; set; }
        public int Version { get; set; }
    }
}
