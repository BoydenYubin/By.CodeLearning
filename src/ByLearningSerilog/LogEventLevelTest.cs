using Serilog;
using Xunit;

namespace ByLearningSerilog
{
    public class LogEventLevelTest
    {
        [Fact]
        public void VerboseLevelTest()
        {
            ///<seealso cref="LoggerConfiguration"/>
            ///Logger的level等级是由MinimumLevel首先决定的
            ///默认值是Information
            ///如果单个Sinks的日志等级小于Information,
            ///其可以输出的日志也只能由MinimumLevel决定
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(path: "log/level.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();

            Log.Logger.Information("Information");
            Log.Logger.Verbose("Verbose");
            Log.Logger.Debug("Debug");
            Log.Logger.Error("Error");
            Log.Logger.Fatal("Fatal");

            //20xx - xxx - xx 22:05:24.410 + 08:00[INF] Information
            //20xx - xxx - xx 22:05:24.415 + 08:00[ERR] Error
            //20xx - xxx - xx 22:05:24.415 + 08:00[FTL] Fatal
        }
        [Fact]
        public void DebugLevelTest()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: "log/level.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
                .CreateLogger();
            Log.Logger.Information("Information");
            Log.Logger.Verbose("Verbose");
            Log.Logger.Debug("Debug");
            Log.Logger.Error("Error");
            Log.Logger.Fatal("Fatal");
            //20xx - xxx - xx 22:06:30.094 + 08:00[INF] Information
            //20xx - xxx - xx 22:06:30.099 + 08:00[ERR] Error
            //20xx - xxx - xx 22:06:30.099 + 08:00[FTL] Fatal
        }
        [Fact]
        public void InformationLevelTest()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: "log/level.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();
            Log.Logger.Information("Information");
            Log.Logger.Verbose("Verbose");
            Log.Logger.Debug("Debug");
            Log.Logger.Error("Error");
            Log.Logger.Fatal("Fatal");
            //20xx - xxx - xx 22:07:42.324 + 08:00[INF] Information
            //20xx - xxx - xx 22:07:42.332 + 08:00[ERR] Error
            //20xx - xxx - xx 22:07:42.332 + 08:00[FTL] Fatal
        }
        [Fact]
        public void ErrorLevelTest()
        {
            ///当查过了MinimumLevel的值时才会正常
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: "log/level.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
                .CreateLogger();
            Log.Logger.Information("Information");
            Log.Logger.Verbose("Verbose");
            Log.Logger.Debug("Debug");
            Log.Logger.Error("Error");
            Log.Logger.Fatal("Fatal");
            //20xx - xxx - xx 22:10:18.822 + 08:00[ERR] Error
            //20xx - xxx - xx 22:10:18.824 + 08:00[FTL] Fatal
        }
        [Fact]
        public void FatalLevelTest()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: "log/level.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal)
                .CreateLogger();
            Log.Logger.Information("Information");
            Log.Logger.Verbose("Verbose");
            Log.Logger.Debug("Debug");
            Log.Logger.Error("Error");
            Log.Logger.Fatal("Fatal");
            //20xx - xxx - xx 22:10:48.808 + 08:00[FTL] Fatal
        }
    }
}