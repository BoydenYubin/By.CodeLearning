using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearningQuartz.CommonJob
{
    public class HelloJob : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			await Console.Out.WriteLineAsync($"{Thread.CurrentThread.ManagedThreadId} : Greetings from HelloJob!");
		}
	}
	public class HelloCopyJob : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			await Console.Out.WriteLineAsync($"{Thread.CurrentThread.ManagedThreadId} : Greetings from HelloCopyJob!");
		}
	}

}
