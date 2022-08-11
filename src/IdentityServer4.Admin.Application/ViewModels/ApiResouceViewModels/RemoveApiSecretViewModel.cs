using System.ComponentModel.DataAnnotations;

namespace ByLearning.Admin.Application.ViewModels.ApiResouceViewModels
{

    public class RemoveApiSecretViewModel
    {
        public RemoveApiSecretViewModel(string resourceName, string type, string value)
        {
            ResourceName = resourceName;
            Type = type;
            Value = value;
        }

        [Required]
        public string ResourceName { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
