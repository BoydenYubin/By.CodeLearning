using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearningConsul.ServiceDiscover
{
    public interface IConsulServiceProvider
    {
        Task<IList<string>> GetServices(string serviceName);
    }
}