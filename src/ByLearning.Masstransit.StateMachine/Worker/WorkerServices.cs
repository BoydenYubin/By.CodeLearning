using ByLearning.Masstransit.StateMachine.EventModel;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearning.Masstransit.StateMachine.Worker
{
    public class WorkerServices : BackgroundService
    {
        private readonly IBus _bus; //注册总线
        private readonly ILogger<WorkerServices> _logger;

        public WorkerServices(IBus bus, ILogger<WorkerServices> logger)
        {
            _bus = bus;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(2000, stoppingToken);
                Guid id = Guid.NewGuid();
                await _bus.Publish<IInitialState>(new { CorrelationId = id });
                //时间间隔1s
                await Task.Delay(2000, stoppingToken);

                Console.WriteLine("Pubed A");
                await _bus.Publish<ITrigerA>(new { CorrelationId = id });
                await Task.Delay(2000, stoppingToken);

                Console.WriteLine("Pubed C");
                await _bus.Publish<ITrigerC>(new { CorrelationId = id });
                await Task.Delay(2000, stoppingToken);

                Console.WriteLine("Pubed D");
                await _bus.Publish<ITrigerD>(new { CorrelationId = id });
                await Task.Delay(2000, stoppingToken);

                await _bus.Publish<IStateProcessed>(new { CorrelationId = id });
            }
        }
    }
}
