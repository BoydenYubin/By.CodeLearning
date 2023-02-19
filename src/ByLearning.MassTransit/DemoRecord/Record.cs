using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace ByLearning.MassTransit.DemoRecord
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
    }

    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        public OrderCreatedEventConsumer(ILogger<OrderCreatedEventConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            //_logger.LogInformation($"Received Order:{context.Message.OrderId}");
            //_logger.LogInformation($"- - - - - - - - - - - - - - - - - - - - - -{DateTimeOffset.Now}");
            return Task.CompletedTask;
        }
    }

    public class Worker : BackgroundService
    {
        private readonly IBus _bus; //注册总线
        private readonly ILogger<Worker> _logger;

        public Worker(IBus bus, ILogger<Worker> logger)
        {
            _bus = bus;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1500, stoppingToken);
                //模拟并发送订单创建事件
                Guid id = Guid.NewGuid();
                await _bus.Publish(new OrderCreatedEvent() { OrderId = id }, stoppingToken);
                //await _bus.Send(new OrderCreatedEvent() { OrderId = id }, stoppingToken);
                //_logger.LogInformation($"Published Order:{id}");
                //时间间隔1s            
            }
        }
    }
}
