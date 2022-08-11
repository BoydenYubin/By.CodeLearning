using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.Domain.Core.Notifications;
using ByLearning.SSO.Application.Interfaces;
using ByLearning.SSO.Application.ViewModels;
using ByLearning.SSO.Domain.ViewModels.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.WebAPI.Controllers
{
    [Route("global-configuration"), Authorize(Policy = "Default")]
    public class GlobalConfigurationController : ApiController
    {
        private readonly IGlobalConfigurationAppService _globalConfigurationSettingsAppService;
        private readonly ISystemUser _systemUser;

        public GlobalConfigurationController(
            INotificationHandler<DomainNotification> notifications,
            IEventBus bus,
            IGlobalConfigurationAppService globalConfigurationSettingsAppService,
            ISystemUser systemUser) : base(notifications, bus)
        {
            _globalConfigurationSettingsAppService = globalConfigurationSettingsAppService;
            _systemUser = systemUser;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ConfigurationViewModel>>> List()
        {
            return ResponseGet(result: await _globalConfigurationSettingsAppService.ListSettings());
        }

        [HttpGet("public-settings"), AllowAnonymous]
        public async Task<ActionResult<PublicSettings>> PublicSettings()
        {
            return ResponseGet(result: await _globalConfigurationSettingsAppService.GetPublicSettings());
        }

        [HttpPut("")]
        public async Task<ActionResult> Update([FromBody] IEnumerable<ConfigurationViewModel> settings)
        {
            await _globalConfigurationSettingsAppService.UpdateSettings(settings);
            return ResponsePutPatch();
        }

        #region  ldap 注释部分
        //[HttpGet("ldap-test")]
        //public ActionResult<LdapConnectionResult> TestLdapSettings([FromQuery] LdapSettingsTestQuery query)
        //{
        //    var ldapTest = new NovelLdapTestConnection(query.Get());
        //    return ResponseGet(ldapTest.Test(query.Username, query.Password));
        //}
        #endregion
    }
}
