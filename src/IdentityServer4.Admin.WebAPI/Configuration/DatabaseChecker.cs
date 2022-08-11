using ByLearning.EntityFrameworkCore.MigrationHelper;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.SSO.Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.WebAPI.Configuration
{
    public class DatabaseChecker
    {
        public static async Task EnsureDatabaseIsReady(IServiceScope serviceScope)
        {
            var services = serviceScope.ServiceProvider;
            var SSOContext = services.GetRequiredService<SSOContext>();

            Log.Information("Testing conection with database");
            await DbHealthChecker.TestConnection(SSOContext);
            Log.Information("Connection successfull");

            Log.Information("Check if database contains Client (ConfigurationDbStore) table");
            await DbHealthChecker.WaitForTable<Client>(SSOContext);

            Log.Information("Check if database contains PersistedGrant (PersistedGrantDbStore) table");
            await DbHealthChecker.WaitForTable<PersistedGrant>(SSOContext);
            Log.Information("Checks done");
        }
    }
}
