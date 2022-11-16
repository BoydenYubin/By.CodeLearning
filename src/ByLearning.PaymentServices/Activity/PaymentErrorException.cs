using System;
using System.Collections.Generic;
using System.Text;

namespace ByLearning.PaymentServices.Activity
{
    public class PaymentErrorException : Exception
    {
        public PaymentErrorException() { }
        public PaymentErrorException(string message) : base(message) { }
    }
}
