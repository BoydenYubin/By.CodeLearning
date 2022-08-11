using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Notifications;
using ByLearning.SSO.Application.Interfaces;
using ByLearning.SSO.Application.ViewModels.UserViewModels;
using IdentityServer4.Admin.WebAPI.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.WebAPI.Controllers
{
    [Route("sign-up"), AllowAnonymous]
    public class SignUpController : ApiController
    {
        private readonly IUserAppService _userAppService;
        private readonly DomainNotificationHandler _notifications;
        private readonly IEventBus _bus;
        private readonly IReCaptchaService _reCaptchaService;

        public SignUpController(
            IUserAppService userAppService,
            INotificationHandler<DomainNotification> notifications,
            IEventBus bus,
            IReCaptchaService reCaptchaService) : base(notifications, bus)
        {
            _userAppService = userAppService;
            _notifications = (DomainNotificationHandler)notifications;
            _bus = bus;
            _reCaptchaService = reCaptchaService;
        }

        [HttpPost, Route("")]
        public async Task<ActionResult<RegisterUserViewModel>> Register([FromBody] RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            if (await _reCaptchaService.IsCaptchaEnabled())
            {
                var captchaSucces = await _reCaptchaService.IsCaptchaPassed();
                if (!captchaSucces)
                {
                    await _bus.Publish(new DomainNotification("Recatcha", "ReCaptcha failed"));
                    return BadRequest(new ValidationProblemDetails(_notifications.GetNotificationsByKey()));
                }
            }

            if (model.ContainsFederationGateway())
                await _userAppService.RegisterWithPasswordAndProvider(model);
            else
                await _userAppService.Register(model);

            model.ClearSensitiveData();
            return ResponsePost("UserData", "Account", null, model);
        }


        [HttpGet, Route("check-username/{suggestedUsername}")]
        public async Task<ActionResult<bool>> CheckUsername(string suggestedUsername)
        {
            var exist = await _userAppService.CheckUsername(suggestedUsername);

            return ResponseGet(exist);
        }

        [HttpGet, Route("check-email/{givenEmail}")]
        public async Task<ActionResult<bool>> CheckEmail(string givenEmail)
        {
            var exist = await _userAppService.CheckUsername(givenEmail);

            return ResponseGet(exist);
        }
    }
}