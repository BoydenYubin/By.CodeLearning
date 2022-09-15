using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearningConsul.ServiceDiscover
{
    public interface IConsulServiceProvider
    {
        Task<IList<string>> GetServices(string serviceName);
        Task<bool> PutKVpairs(string key, string values);
        Task<string> GetKVvalues(string key);
    }
}