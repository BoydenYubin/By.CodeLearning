using Quartz;
using Shouldly;
using System;

namespace ByLearningQuartz.Triger
{
    public class SimpleTrigerTest : ITestWork
    {
        public void RunAsync()
        {
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(builder =>
                {
                    builder.WithInterval(TimeSpan.FromDays(1))
                    .WithRepeatCount(10);
                })
                .Build();
            trigger.GetType().Name.ShouldContain(nameof(Quartz.Impl.Triggers.SimpleTriggerImpl));
        }
    }
}
