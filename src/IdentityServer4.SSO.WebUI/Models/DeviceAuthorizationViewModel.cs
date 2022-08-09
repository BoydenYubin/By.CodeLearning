using IdentityServer4.SSO.WebUI.Controllers.Consent;

namespace IdentityServer4.SSO.WebUI.Models
{
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        public string UserCode { get; set; }
        public bool ConfirmUserCode { get; set; }
    }
}