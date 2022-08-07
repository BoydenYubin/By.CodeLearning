using System.ComponentModel.DataAnnotations;

namespace ByLearning.SSO.Application.ViewModels.RoleViewModels
{
    public class RemoveRoleViewModel
    {
        public RemoveRoleViewModel(string name)
        {
            Name = name;
        }

        [Required]
        public string Name { get; set; }
    }
}
