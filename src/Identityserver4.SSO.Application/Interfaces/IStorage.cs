using ByLearning.SSO.Application.ViewModels;
using System.Threading.Tasks;

namespace ByLearning.SSO.Application.Interfaces
{
    public interface IStorage
    {
        Task<string> Upload(FileUploadViewModel image);
        Task Remove(string filename, string virtualLocation);
    }

}
