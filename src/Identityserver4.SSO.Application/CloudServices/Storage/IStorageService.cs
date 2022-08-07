using System.Threading.Tasks;
using ByLearning.SSO.Application.ViewModels;

namespace ByLearning.SSO.Application.CloudServices.Storage
{
    public interface IStorageService
    {
        Task<string> Upload(FileUploadViewModel file);
        Task RemoveFile(string fileName, string virtualLocation);
    }
}