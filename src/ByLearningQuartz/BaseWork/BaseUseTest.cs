using ByLearningQuartz.CommonJob;
using Quartz;
using Quartz.Impl;

namespace ByLearningQuartz.BaseWork
{
    public class BaseUseTest : ITestWork
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
            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}
