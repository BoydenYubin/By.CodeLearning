using System.Threading.Tasks;

namespace IdentityServer4.Admin.WebAPI.Interfaces
{
    public interface IReCaptchaService
    {
        Task<bool> IsCaptchaPassed();
        Task<bool> IsCaptchaEnabled();
    }
}