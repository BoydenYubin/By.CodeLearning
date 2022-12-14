using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ByProject.AutoUpgrade.SimpleServer
{
    public class UpgradeController : ControllerBase
    {
        public Task<UpgradeResponse> Index(string version)
        {
            return Task.FromResult(new UpgradeResponse()
            {
                DownloadLink = "http://10.0.3.17:8000/netcoreapp3.1.zip",
                Version = "1.1.2"
            });
        }
    }
}
