using System;
using static Bullseye.Targets;

namespace ByLearningBullseye
{
    public class EnumerableRuning : IRuning
    {
        public void Run(string[] args)
        {
            args = new string[] { "eat-biscuits" };
            Target("eat-biscuits",ForEach("digestives", "chocolate hob nobs"),
                            biscuits => Console.WriteLine($"Mmm...{biscuits}! Nom nom."));
            RunTargetsAndExit(args);
        }
    }
}
