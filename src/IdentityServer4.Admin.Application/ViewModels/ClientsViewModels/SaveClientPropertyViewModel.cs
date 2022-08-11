using System.ComponentModel.DataAnnotations;

namespace ByLearning.Admin.Application.ViewModels.ClientsViewModels
{
    public class SaveClientPropertyViewModel
    {
        [Required]
        public string Value { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string ClientId { get; set; }
    }
}