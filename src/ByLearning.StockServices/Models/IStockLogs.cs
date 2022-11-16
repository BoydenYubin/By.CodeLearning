using System;
using System.Collections.Generic;
using System.Text;

namespace ByLearning.StockServices.Models
{
    public interface IStockLogs
    {
        string CustomerName { get; set; }
        public Guid RequestID { get; set; }
        public IDictionary<string, int> OrderInfo { get; set; }
    }
}
