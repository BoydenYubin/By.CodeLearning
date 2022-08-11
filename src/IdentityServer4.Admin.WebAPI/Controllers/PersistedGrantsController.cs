using AspNetCore.IQueryable.Extensions;
using ByLearning.Admin.Application.Interfaces;
using ByLearning.Admin.Application.ViewModels;
using ByLearning.DDD.Domain.Core.ViewModels;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.Domain.Core.Notifications;
using ByLearning.Domain.Core.Util;
using ByLearning.SSO.AspNetIdentity.Models.Identity;
using ByLearning.SSO.Domain.ViewModels.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.WebAPI.Controllers
{
    [Route("persisted-grants"), Authorize(Policy = "Default")]
    public class PersistedGrantsController : ApiController
    {
        private readonly IPersistedGrantAppService _persistedGrantAppService;
        private readonly ISystemUser _systemUser;
        private readonly UserManager<UserIdentity> _manager;

        public PersistedGrantsController(
            INotificationHandler<DomainNotification> notifications,
            IEventBus bus,
            IPersistedGrantAppService persistedGrantAppService,
            ISystemUser systemUser,
            UserManager<UserIdentity> manager) : base(notifications, bus)
        {
            _persistedGrantAppService = persistedGrantAppService;
            _systemUser = systemUser;
            _manager = manager;
        }

        [HttpGet, Route("")]
        public async Task<ActionResult<ListOf<PersistedGrantViewModel>>> List([Range(1, 50)] int? limit = 10, [Range(1, int.MaxValue)] int? offset = 0)
        {
            // Search for grants
            var searchPersisted = new PersistedGrantSearch()
            {
                Limit = limit,
                Offset = offset
            };
            var persistedGrants = await _persistedGrantAppService.GetPersistedGrants(searchPersisted);

            // Get additional data from users
            var usersIds = persistedGrants.Collection.Select(s => s.SubjectId).ToArray();
            var search = new UserSearch<string>()
            {
                Id = usersIds,
                Limit = limit,
                Offset = offset
            };
            var users = await _manager.Users.Apply(search).ToListAsync();

            // Update addional data
            var collection = persistedGrants.Collection.ToList();
            foreach (var persistedGrantViewModel in collection)
            {
                var user = users.FirstOrDefault(u => u.Id == persistedGrantViewModel.SubjectId);
                if (user == null) continue;

                persistedGrantViewModel.UpdateUserInfo(user.UserName);
            }

            persistedGrants.Collection = collection;
            // truncate data for non administration users
            if (!User.IsInRole("Administrator") && !User.HasClaim(c => c.Type == "is4-manager"))
            {
                foreach (var persistedGrantViewModel in persistedGrants.Collection)
                {
                    if (persistedGrantViewModel.Email == _systemUser.Username)
                        continue;

                    persistedGrantViewModel.Email = persistedGrantViewModel.Email?.TruncateSensitiveInformation();
                    persistedGrantViewModel.Data = persistedGrantViewModel.Data?.TruncateSensitiveInformation();
                }
            }

            return ResponseGet(new ListOf<PersistedGrantViewModel>(persistedGrants.Collection, persistedGrants.Total));
        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            var model = new RemovePersistedGrantViewModel(id.FromBase64UrlSafe().FromUtf8Bytes());
            await _persistedGrantAppService.Remove(model);
            return ResponseDelete();
        }


    }
}
