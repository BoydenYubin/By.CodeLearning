using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByLerning.SignalR.Controllers
{
    [Authorize]
    public class HomeControllers : ControllerBase
    {
        [HttpGet]
        public ActionResult Test()
        {
            return Ok("test");
        }
    }
}
