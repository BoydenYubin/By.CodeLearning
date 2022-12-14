using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ByProject.AutoUpgrade.WeApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        private readonly IAutoUpgradeProvider _autoUpgrade;

        public TestController(IAutoUpgradeProvider autoUpgrade)
        {
            _autoUpgrade = autoUpgrade;
        }

        [HttpGet]
        [Route("upgrade")]
        public async Task<string> Upgrade()
        {
            var result = await _autoUpgrade.CheckUpdateAsync();
            return $"status:{result.Status},message{result.Message}";
        }

        [HttpGet]
        [Route("version")]
        public Task<string> Version()
        {
            return Task.FromResult("1.1.2");
        }
    }
}
