using ByLearningLogger.Util;
using Serilog;
using Xunit;

namespace ByLearningSerilog.WirteToSeq
{
    public class WriteToSeqTest
    {
        [Fact]
        public void HowToUseSeqTest()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Seq(GetConnectString.GetSeqUrl(), apiKey: GetConnectString.GetSeqAPIKey())
                .CreateLogger();
            var baseClass = new BaseClass()
            {
                Id = 12,
                Name = "boyden"
            };
            Log.Information("{ID} + {Name}", baseClass.Id, baseClass.Name);
            Log.CloseAndFlush();
        }
    }
}
