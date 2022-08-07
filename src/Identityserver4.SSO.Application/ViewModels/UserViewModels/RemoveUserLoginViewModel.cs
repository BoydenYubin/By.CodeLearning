using System.ComponentModel.DataAnnotations;

namespace ByLearning.SSO.Application.ViewModels.UserViewModels
{
    public class RemoveUserLoginViewModel
    {
        public RemoveUserLoginViewModel(string username, string loginProvider, string providerKey)
        {
            Username = username;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }

        [Required]
        public string Username { get; set; }
        [Required]
        public string LoginProvider { get; set; }
        [Required]
        public string ProviderKey { get; set; }
    }
}