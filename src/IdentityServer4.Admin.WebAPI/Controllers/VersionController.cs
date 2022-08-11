using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.Admin.WebAPI.Controllers
{
    [Route("version"), ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public string Get() => "full";

    }
}
