using ByLearning.SSO.Domain.ViewModels;
using IdentityServer4.SSO.Domain.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ByLearning.SSO.Application.ViewModels.UserViewModels
{
    public class UserListViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Name { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public string Id { get; set; }

        internal void UpdateMetadata(IEnumerable<Claim> claim)
        {
            Picture = claim.ValueOf(JwtClaimTypes.Picture);
            Name = claim.ValueOf(JwtClaimTypes.GivenName);
        }
    }

}