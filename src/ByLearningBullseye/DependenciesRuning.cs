using System;
using static Bullseye.Targets;

namespace ByLearningBullseye
{
    public class DependenciesRuning : IRuning
    {
        public void Run(string[] args)
        {
            Target("make-tea", () => Console.WriteLine("Tea made."));
            Target("drink-tea", DependsOn("make-tea"), () => Console.WriteLine("Ahh... lovely!"));
            Target("walk-dog", () => Console.WriteLine("Walkies!"));
            Target("default", DependsOn("drink-tea", "walk-dog"));
            RunTargetsAndExit(args);
        }
    }
}
