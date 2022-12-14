using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ByProject.AutoUpgrade
{
    public class AutoUpgradeProvider : IAutoUpgradeProvider
    {
        private readonly UpgradeOptions _options;
        private readonly ILogger<AutoUpgradeProvider> _logger;

        public AutoUpgradeProvider(ILogger<AutoUpgradeProvider> logger, IOptionsMonitor<UpgradeOptions> options)
        {
            _options = options.CurrentValue;
            _logger = logger;
        }
        public async Task<UpgradeResult> CheckUpdateAsync()
        {
            if (_options == null)
            {
                _logger.LogError("Should config the 'AutoUpgrade' section in configuration file or Configuration Center");
                throw new NullReferenceException("Should config the 'AutoUpgrade' section in configuration file or Configuration Center");
            }
            ///Initinal the upgrade options
            var upgradeUrl = _options.UpgradeUrl ?? "http://localhost:12345";
            var upgradeEndpoint = _options.UpgradeEndpoint ?? "default";
            var curVersion = new Version(_options.Version);
            int timeout = _options.Timeout < 3000 ? 10000 : _options.Timeout;
            string responseContent;

            #region Get upgrade response by httpclient
            try
            {
                using (HttpClient upgradeClient = new HttpClient())
                {
                    //To do => config the httpclient by injection the action of func by servicecollection
                    var token = new CancellationTokenSource(timeout);
                    //this may throw exceptions, caused by as network connectivity, DNS failure
                    //server certificate validation or timeout
                    var response = await upgradeClient.GetAsync($"{upgradeUrl}/{upgradeEndpoint}?version={curVersion}", token.Token);
                    _logger.LogDebug($"{response.StatusCode},{response.Content.ReadAsStringAsync().Result}");
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return new UpgradeResult()
                        {
                            Status = UpgradeStatus.ServerError,
                            Message = $"Inspect upgrade failed! Response code: {response.StatusCode} from {upgradeUrl}!"
                        };
                    }
                    responseContent = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogDebug($"{typeof(HttpRequestException).Name}, message:{ex.Message}!");
                return new UpgradeResult()
                {
                    Status = UpgradeStatus.ServerError,
                    Message = $"Can't connect the server: {upgradeUrl}, with exception {ex.Message}!"
                };
            }
            catch (Exception e)
            {
                _logger.LogDebug($"{typeof(Exception).Name}, message:{e.Message}!");
                return new UpgradeResult()
                {
                    Status = UpgradeStatus.ServerError,
                    Message = $"ServerError with exception: {e.Message}!"
                };
            }
            if (string.IsNullOrEmpty(responseContent))
            {
                return new UpgradeResult()
                {
                    Status = UpgradeStatus.ServerError,
                    Message = $"Bad Response Info From Server!"
                };
            }
            #endregion

            ///<seealso cref="JsonSerializerDefaults.Web"/>
            ///这里注意序列化时需要配置默认的序列化器为web，注意文本形式
            var downloadResponse = JsonSerializer.Deserialize<UpgradeResponse>(responseContent,new JsonSerializerOptions(JsonSerializerDefaults.Web));
            if (downloadResponse == null || string.IsNullOrEmpty(downloadResponse.DownloadLink))
            {
                return new UpgradeResult()
                {
                    Status = UpgradeStatus.EmptyUpgrade,
                    Message = $"Get Empty Upgrade Info From Server!"
                };
            }

            if (new Version(downloadResponse.Version) <= curVersion)
            {
                return new UpgradeResult()
                {
                    Status = UpgradeStatus.VersionLatest,
                    Message = "Current version is latest!"
                };
            }
            //注意Directory.GetCurrentDirectory()在windows下由其他程序调用可能程序不对
            var shScript = $@"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}update.sh";
            try
            {
                if (!Process.Start(shScript, downloadResponse.DownloadLink).WaitForExit(timeout))
                {
                    return new UpgradeResult()
                    {
                        Status = UpgradeStatus.UpgradeTimeout,
                        Message = $"Upgrade failed Timeout in {timeout / 1000} seconds!"
                    };
                }
            }
            catch (FileNotFoundException)
            {
                return new UpgradeResult()
                {
                    Status = UpgradeStatus.UpgradeFailed,
                    Message = "Lost 'update.sh' Script!"
                };
            }
            catch (Exception e)
            {
                return new UpgradeResult()
                {
                    Status = UpgradeStatus.UpgradeFailed,
                    Message = $"Upgrade failed with error {e.Message}"
                };
            }
            return new UpgradeResult()
            {
                Status = UpgradeStatus.UpgradeSuccess,
                Message = $"Upgrade Successed!"
            };
        }
    }
}
