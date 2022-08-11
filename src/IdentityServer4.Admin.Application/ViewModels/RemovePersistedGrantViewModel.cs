using System.ComponentModel.DataAnnotations;

namespace ByLearning.Admin.Application.ViewModels
{
    public class RemovePersistedGrantViewModel
    {
        public RemovePersistedGrantViewModel(string key)
        {
            Key = key;
        }

        [Required]
        public string Key { get; set; }
    }
}