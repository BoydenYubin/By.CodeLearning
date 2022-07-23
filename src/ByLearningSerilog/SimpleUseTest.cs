using ByLearningLogger.Util;
using Serilog;
using Serilog.Core;
using Serilog.Filters;
using System;
using System.Threading;
using Xunit;
//  ______     ______     __  __     _____     ______     __   __    
// /\  == \   /\  __ \   /\ \_\ \   /\  __-.  /\  ___\   /\ "-.\ \   
// \ \  __<   \ \ \/\ \  \ \____ \  \ \ \/\ \ \ \  __\   \ \ \-.  \  
//  \ \_____\  \ \_____\  \/\_____\  \ \____-  \ \_____\  \ \_\\"\_\ 
//   \/_____/   \/_____/   \/_____/   \/____/   \/_____/   \/_/ \/_/ 

namespace ByLearningSerilog
{
    public class SimpleUseTest
    {
        [Fact]
        public void SimpleUse()
        {
            Log.Logger = new LoggerConfiguration()
                /* write to file configuration
                 * LogEventLevel restrictedToMinimumLevel,
                 * string outputTemplate,
                 * IFormatProvider formatProvider, 
                 * long? fileSizeLimitBytes, 
                 * LoggingLevelSwitch levelSwitch, 
                 * bool buffered, 
                 * bool shared, 
                 * TimeSpan? flushToDiskInterval
                 */
                .WriteTo.File(path: "logs\\test.txt", rollingInterval: RollingInterval.Day)
                /* WriteTo Seq configuration
                 * string serverUrl, 
                 * LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, 
                 * int batchPostingLimit = 1000, 
                 * TimeSpan? period = null, 
                 * string apiKey = null, 
                 * string bufferBaseFilename = null,
                 * long? bufferSizeLimitBytes = null, 
                 * long? eventBodyLimitBytes = 262144L, 
                 * LoggingLevelSwitch controlLevelSwitch = null,
                 * HttpMessageHandler messageHandler = null, 
                 * long? retainedInvalidPayloadsLimitBytes = null,
                 * bool compact = false,
                 * int queueSizeLimit = 100000
                 */
                .WriteTo.Seq(GetConnectString.GetSeqUrl(), apiKey: GetConnectString.GetSeqAPIKey())
                .CreateLogger();
            // XXXX-XX-XX 20:00:24.755 +08:00 [INF] Hello, world!
            Log.Information("Hello, world!");
            Log.CloseAndFlush();
        }
        /// <summary>
        /// 默认输出模板是{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}
        /// Level:u3  消息等级取前三位
        /// </summary>
        [Fact]
        public void EnrichTest()
        {
            Log.Logger = new LoggerConfiguration()
                /*
                 * Enricher的本质就是"" + object
                 * 同时查看下一条测试
                 */
                .Enrich.With(new ThreadIdEnricher())
                .WriteTo.File(path: "logs\\test.txt",
                              outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}",
                              rollingInterval: RollingInterval.Day)
                 .WriteTo.Seq(GetConnectString.GetSeqUrl(), apiKey: GetConnectString.GetSeqAPIKey())
                .CreateLogger();
            // 20:16 [Information] (13) Hello, world!
            Log.Information("Hello, world!");
            Log.CloseAndFlush();
        }

        [Fact]
        public void EnrichWithPropertyTest()
        {
            //Enrich必须结合outputTemplate使用
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("Guid", Guid.NewGuid())
                .WriteTo.File(path: "logs\\test.txt",
                              outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Guid}) {Message:2j}{NewLine}{Exception}",
                              rollingInterval: RollingInterval.Day)
                //当写入Seq，  Enrich 会变成一个Property      
                .WriteTo.Seq(GetConnectString.GetSeqUrl(), apiKey: GetConnectString.GetSeqAPIKey())
                .CreateLogger();
            Log.Information("Hello, world!");
            Log.CloseAndFlush();
        }

        [Fact]
        public void FilterTest()
        {
            Log.Logger = new LoggerConfiguration()
                /* Filter用法
                 * With<TFilter>()
                 * ByExcluding(Func<LogEvent, bool>
                 * ByIncludingOnly(Func<LogEvent, bool>
                 * -------------------------------------------------
                 * Matching 用法
                 * Func<LogEvent, bool> FromSource<TSource>  Matches events from the specified source type
                 * Func<LogEvent, bool> FromSource(string source)  
                 * Func<LogEvent, bool> WithProperty(string propertyName）
                 * Func<LogEvent, bool> WithProperty(string propertyName, object scalarValue） scalarValue：The property value to match; must be a scalar type. Null is allowed
                 * Func<LogEvent, bool> WithProperty<TScalar>(string propertyName, Func<TScalar, bool> predicate）
                 */
                .Filter.ByExcluding(Matching.WithProperty<int>("Count", p => p > 10))
                .Enrich.WithProperty("Count", new Random().Next(0,20))
                .WriteTo.File(path: "logs\\test.txt",
                outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Count}) {Message}{NewLine}{Exception}",
                              rollingInterval: RollingInterval.Day)
                .WriteTo.Seq(GetConnectString.GetSeqUrl(), apiKey: GetConnectString.GetSeqAPIKey())
                .CreateLogger();
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                Log.Logger.Information("Hello, world!");
            }
            Log.CloseAndFlush();
        }

        [Fact]
        public void SourceContextTest()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\test.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{SourceContext}{NewLine}")
                 .WriteTo.Seq(GetConnectString.GetSeqUrl(), apiKey: GetConnectString.GetSeqAPIKey())
                .CreateLogger();
            // file中只有空行，Sea中有消息
            Log.Information("Hello world"); //无相关类信息
            var mylogger = Log.Logger.ForContext<BaseClass>();
            // file中 ByLearningSerilog.BaseClass
            mylogger.Information("Hello world");//有相关类信息
            Log.CloseAndFlush();
        }

        [Fact]
        public void SourceContextCorrelationTest()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\test.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{SourceContext}{BassId}{Message}{NewLine}")
                 .WriteTo.Seq(GetConnectString.GetSeqUrl(), apiKey: GetConnectString.GetSeqAPIKey())
                .CreateLogger();
            BaseClass @base = new BaseClass { Id = 5, Name = "test" };
            var mylogger = Log.ForContext("BassId", @base.Id);
            //5Hello world
            mylogger.Information("Hello world");
            Log.CloseAndFlush();
        }
        [Fact]
        public void ShouldControlledByLevelSwich()
        {
            LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Verbose);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Seq(GetConnectString.GetSeqUrl(), apiKey: GetConnectString.GetSeqAPIKey())
                .WriteTo.File("logs\\test.txt", rollingInterval: RollingInterval.Day).CreateLogger();
            Log.Verbose("Should exist");
            levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
            Log.Verbose("Should not exist");
            Log.CloseAndFlush();
        }
        /*
         * Tip: when logging to sinks that use a text format, such as Serilog.Sinks.Console,
         * you can include {Properties} in the output template to print out all contextual properties not otherwise included.
         */
    }
}
