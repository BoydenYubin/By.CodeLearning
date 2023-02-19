using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ByLearning.Masstransit.WebApi.TrackingTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrackingController : ControllerBase
    {

        private readonly ILogger<TrackingController> _logger;
        private readonly IBus _bus;

        public TrackingController(ILogger<TrackingController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpGet]
        [Route("publish")]
        public string Publish()
        {
            Guid id = Guid.NewGuid();
            _bus.Publish(new OrderCreatedEvent() { OrderId = id });
            return $"Publish finished: {DateTime.Now}";
        }
    }
}
