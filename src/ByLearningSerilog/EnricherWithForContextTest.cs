using Serilog;
using Serilog.Context;
using Xunit;

namespace ByLearningSerilog
{
    public class EnricherWithForContextTest
    {
        [Fact]
        public void ForContextTest()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(path: "logs\\test.txt",
                              outputTemplate: "{Timestamp:HH:mm} [{Level}] [{ThreadId}] {Message}{NewLine}{Exception}",
                              rollingInterval: RollingInterval.Day)
                .CreateLogger();
            // 20:16 [Information] (13) Hello, world!
            using (LogContext.Push(new ThreadIdEnricher()))
            {
                ///同样也可以使用<seealso cref="LogContext.PushProperty(string, object, bool)"/>
                //22:53 [Information] [13] Hello, world!
                //此部分包括LogContext Push的ThreadIdEnricher
                Log.Information("Hello, world!");
            }
            //22:53[Information][] Hello, world!
            //此部分不包括LogContext Push的ThreadIdEnricher
            Log.Information("Hello, world!");
            Log.CloseAndFlush();
        }
    }
}
