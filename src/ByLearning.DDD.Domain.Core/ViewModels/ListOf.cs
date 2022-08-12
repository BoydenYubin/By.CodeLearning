using System.Collections.Generic;

namespace ByLearning.DDD.Domain.Core.ViewModels
{
    public class ListOf<T> where T : class
    {
        public ListOf(IEnumerable<T> collection, int total)
        {
            Collection = collection;
            Total = total;
        }

        public IEnumerable<T> Collection { get; set; }

        public int Total { get; set; }
    }
}
