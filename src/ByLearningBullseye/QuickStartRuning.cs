using System;
using static Bullseye.Targets;

namespace ByLearningBullseye
{
    public class QuickStartRuning : IRuning
    {
        public void Run(string[] args)
        {
            Target("default", () => Console.WriteLine("Hello Bullseye!"));
            RunTargetsAndExit(args);
        }
    }
}
