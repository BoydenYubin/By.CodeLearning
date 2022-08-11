using System.ComponentModel.DataAnnotations;

namespace ByLearning.Admin.Application.ViewModels.ApiResouceViewModels
{
    public class RemoveApiResourceViewModel
    {
        public RemoveApiResourceViewModel(string name)
        {
            Name = name;
        }

        [Required]
        public string Name { get; set; }
    }
}