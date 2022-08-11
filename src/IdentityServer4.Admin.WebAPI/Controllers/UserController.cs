using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Notifications;
using ByLearning.SSO.Application.Interfaces;
using ByLearning.SSO.Application.ViewModels.UserViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.WebAPI.Controllers
{

    [Route("user"), AllowAnonymous]
    public class UserController : ApiController
    {
        private readonly IUserAppService _userAppService;

        public UserController(
            IUserAppService userAppService,
            INotificationHandler<DomainNotification> notifications,
            IEventBus bus) : base(notifications, bus)
        {
            _userAppService = userAppService;
        }

        [HttpPost, Route("{username}/password/forget")]
        public async Task<ActionResult> ForgotPassword(string username)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }
            var model = new ForgotPasswordViewModel(username);
            await _userAppService.SendResetLink(model);

            return Ok();
        }

        [HttpPost, Route("{username}/password/reset")]
        public async Task<ActionResult> ResetPassword(string username, [FromBody]ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            model.Email = username;
            await _userAppService.ResetPassword(model);

            return Ok();
        }

        [HttpPost, Route("{username}/confirm-email")]
        public async Task<ActionResult> ConfirmEmail(string username, [FromBody] ConfirmEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            model.Email = username;
            await _userAppService.ConfirmEmail(model);
            return Ok();
        }

    }
}