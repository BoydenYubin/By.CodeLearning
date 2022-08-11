using ByLearning.Admin.Application.ViewModels.ClientsViewModels;
using ByLearning.SSO.Application.ViewModels;
using IdentityServer4.Models;

namespace IdentityServer4.Admin.WebAPI.ViewModel
{
    public class SaveClientWithLogoViewModel : SaveClientViewModel
    {
        public FileUploadViewModel Logo { get; set; }
    }
    public class UpdateClientWithLogoViewModel : Client
    {
        public FileUploadViewModel Logo { get; set; }
    }
}
