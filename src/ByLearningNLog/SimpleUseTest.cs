using NLog;
using Xunit;

namespace ByLearningNLog
{
    public class SimpleUseTest
    {
        [Fact]
        public void SimpleConnectTest()
        {
            MappedDiagnosticsContext.Set("nihao", "woshi");
            Logger Logger = LogManager.GetCurrentClassLogger();
            Logger.Info("Hello World");
        }
    }
}
