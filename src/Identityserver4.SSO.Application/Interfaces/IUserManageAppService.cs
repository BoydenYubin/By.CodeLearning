using ByLearning.DDD.Domain.Core.ViewModels;
using ByLearning.SSO.Application.EventSourcedNormalizers;
using ByLearning.SSO.Application.ViewModels;
using ByLearning.SSO.Application.ViewModels.RoleViewModels;
using ByLearning.SSO.Application.ViewModels.UserViewModels;
using ByLearning.SSO.Domain.Interfaces;
using ByLearning.SSO.Domain.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearning.SSO.Application.Interfaces
{
    public interface IUserManageAppService : IDisposable
    {
        Task<UserViewModel> FindByUsernameAsync(string username);
        Task<UserViewModel> FindByEmailAsync(string email);
        Task<UserViewModel> FindByProviderAsync(string provider, string providerUserId);
        Task<ListOf<EventHistoryData>> GetEvents(string username, PagingViewModel paging);
        Task<UserViewModel> GetUserDetails(string username);
        Task<IEnumerable<ClaimViewModel>> GetClaims(string userName);
        Task<bool> SynchronizeClaims(string username, IEnumerable<ClaimViewModel> claims);
        Task<IEnumerable<RoleViewModel>> GetRoles(string userName);
        Task<IEnumerable<UserLoginViewModel>> GetLogins(string userName);
        Task<IEnumerable<UserListViewModel>> GetUsersInRole(string role);
        Task<ListOf<UserListViewModel>> SearchUsersByProperties(UserFindByProperties findByProperties);
        Task<ListOf<UserListViewModel>> Search(IUserSearch search);
        Task<bool> UpdateProfile(UserViewModel model);
        Task<bool> UpdateProfilePicture(ProfilePictureViewModel model);
        Task<bool> ChangePassword(ChangePasswordViewModel model);
        Task<bool> CreatePassword(SetPasswordViewModel model);
        Task<bool> RemoveAccount(RemoveAccountViewModel model);
        Task<bool> HasPassword(string userId);
        Task<bool> UpdateUser(UserViewModel model);
        Task<bool> SaveClaim(SaveUserClaimViewModel model);
        Task<bool> RemoveClaim(RemoveUserClaimViewModel model);
        Task<bool> RemoveRole(RemoveUserRoleViewModel model);
        Task<bool> SaveRole(SaveUserRoleViewModel model);
        Task<bool> RemoveLogin(RemoveUserLoginViewModel model);
        Task<bool> ResetPassword(AdminChangePasswordViewodel model);


    }
}
