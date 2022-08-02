using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ByLearning.IdentityServer4.HybirdAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpGet("getUserClaims")]
        [Authorize]
        public IActionResult GetUserClaims()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
