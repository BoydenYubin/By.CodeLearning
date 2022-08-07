using System.Collections.Generic;

namespace Identityserver4.SSO.Application.ViewModels.Common
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
