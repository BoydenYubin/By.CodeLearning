using System.ComponentModel.DataAnnotations;

namespace ByLearning.Admin.Application.ViewModels.ClientsViewModels
{
    public class RemoveClientViewModel
    {
        public RemoveClientViewModel(string clientId)
        {
            ClientId = clientId;
        }

        [Required]
        public string ClientId { get; set; }

    }
}