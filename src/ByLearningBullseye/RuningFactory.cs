using System;
using System.Collections.Generic;
using System.Text;

namespace ByLearningBullseye
{
    public enum Towards : byte
    {
        DefaultRuning = 0,
        QuickStartRuning = 1,
        DependenciesRuning = 2,
        EnumerableRuning = 3,
        DotnetVersionRunning = 4
    }
    public class RuningFactory : IRuningFactory
    {
        public IRuning CreateRuning(Towards towards)
        {
            IRuning runing;
            switch (towards)
            {
                case Towards.QuickStartRuning:
                    runing = new QuickStartRuning();
                    break;
                case Towards.DependenciesRuning:
                    runing = new DependenciesRuning();
                    break;
                case Towards.EnumerableRuning:
                    runing = new EnumerableRuning();
                    break;
                case Towards.DotnetVersionRunning:
                    runing = new DotnetVersionRunning();
                    break;
                default:
                    runing = new DefaultRuning();
                    break;
            }
            return runing;
        }
    }
}
