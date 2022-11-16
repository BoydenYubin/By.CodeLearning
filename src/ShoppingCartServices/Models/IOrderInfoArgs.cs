using System;
using System.Collections.Generic;

namespace OrderService.Models
{
    public interface IOrderInfoArgs
    {
        string CustomerName { get; set; }
        public Guid RequestID { get; set; }
        public IDictionary<string, int> OrderInfo { get; set; }
    }
}