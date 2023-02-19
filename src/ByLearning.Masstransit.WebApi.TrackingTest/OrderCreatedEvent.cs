using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ByLearning.Masstransit.WebApi.TrackingTest
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
}
