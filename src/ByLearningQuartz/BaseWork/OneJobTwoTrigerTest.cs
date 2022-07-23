using ByLearningQuartz.CommonJob;
using Quartz;
using Quartz.Impl;

namespace ByLearningQuartz.BaseWork
{
    /// <summary>
    /// 一个job对应两个triger的注意事项
    /// 1、job必须持久化
    /// 2、需要将job增加到IScheduler
    /// 3、建立两个triger并匹配job
    /// 4、_scheduler.ScheduleJob(triger)
    /// </summary>
    public class OneJobTwoTrigerTest : ITestWork
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

            await _scheduler.AddJob(job, false);
            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .ForJob("myJob", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
            .Build();



            ITrigger trigger2 = TriggerBuilder.Create()
                .WithIdentity("myTrigger2", "group1")
                .ForJob("myJob", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(3)
                    .RepeatForever())
            .Build();

            await _scheduler.ScheduleJob(trigger2);
            await _scheduler.ScheduleJob(trigger);
        }
    }
}
