using ByLearningQuartz.CommonJob;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearningQuartz.BaseWork
{
    public class JobListenerTest : ITestWork
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

            _scheduler.ListenerManager.AddJobListener(new MyJobLisner(), KeyMatcher<JobKey>.KeyEquals(new JobKey("myJob", "group1")));
            _scheduler.ListenerManager.AddTriggerListener(new MyTrigerLisner(), KeyMatcher<TriggerKey>.KeyEquals(new TriggerKey("myTrigger", "group1")));
            await _scheduler.ScheduleJob(job, trigger);
        }
    }

    public class MyJobLisner : IJobListener
    {
        public string Name => "MyJobLisner";

        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            //当此job被投票不执行时，执行此操作
            await Console.Out.WriteLineAsync("Job 被投票不执行");
        }

        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            await Console.Out.WriteLineAsync("Job 执行前");
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            await Console.Out.WriteLineAsync("Job 执行后");
        }
    }
}
