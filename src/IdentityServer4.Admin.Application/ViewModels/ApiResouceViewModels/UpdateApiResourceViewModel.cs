using IdentityServer4.Models;

namespace ByLearning.Admin.Application.ViewModels.ApiResouceViewModels
{
    public class UpdateApiResourceViewModel : ApiResource
    {
        public string OldApiResourceId { get; set; }
    }
}
