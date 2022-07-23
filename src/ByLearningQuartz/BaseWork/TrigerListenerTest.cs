using ByLearningQuartz.CommonJob;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearningQuartz.BaseWork
{
    public class TrigerListenerTest : ITestWork
    {
        private StdSchedulerFactory _factory;
        private IScheduler _scheduler;
        public async void RunAsync()
        {
            // construct a scheduler factory
            _factory = new StdSchedulerFactory();
            // get a scheduler
            _scheduler = await _factory.GetScheduler();
            await _scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("myJob", "group1")
                .StoreDurably()
                .Build();
            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
            .Build();

            _scheduler.ListenerManager.AddTriggerListener(new MyTrigerLisner(), KeyMatcher<TriggerKey>.KeyEquals(new TriggerKey("myTrigger", "group1")));

            await _scheduler.ScheduleJob(job, trigger);
        }
    }
    public class MyTrigerLisner : ITriggerListener
    {
        public string Name => "MyTrigerLisner";

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
        {
            Console.WriteLine(trigger.JobKey.Name + " Finished!");
            return Task.CompletedTask;
        }
        
        public async Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            await Console.Out.WriteLineAsync(trigger.JobKey.Name + " being triggered!");
        }
        
        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
        /// <summary>
        /// 当返回结果为true时，则投票结果为不执行任务
        /// 当返回结果为false时，则头片结果为继续执行任务
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            //return Task.FromResult(true);
            return Task.FromResult(true);
        }
    }
}
