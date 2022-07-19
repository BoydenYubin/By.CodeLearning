using System.Collections.Generic;
using static Bullseye.Targets;

namespace ByLearningBullseye
{
    public class DotnetVersionRunning : IRuning
    {
        public void Run(string[] args)
        {
            Target("version", () =>
            {
                SimpleExec.Command.Run("dotnet", "--version", echoPrefix: "Test");
            });
            RunTargetsAndExit(new List<string> { "version"});
        }
    }
}
