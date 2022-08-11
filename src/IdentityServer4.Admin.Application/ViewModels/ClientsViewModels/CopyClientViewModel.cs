using System.ComponentModel.DataAnnotations;

namespace ByLearning.Admin.Application.ViewModels.ClientsViewModels
{
    public class CopyClientViewModel
    {
        public CopyClientViewModel(string clientId)
        {
            ClientId = clientId;
        }

        [Required]
        public string ClientId { get; set; }

    }
}