using System.ComponentModel.DataAnnotations;

namespace ByLearning.Admin.Application.ViewModels.IdentityResourceViewModels
{
    public class RemoveIdentityResourceViewModel
    {
        public RemoveIdentityResourceViewModel(string name)
        {
            Name = name;
        }

        [Required]
        public string Name { get; set; }
    }
}
