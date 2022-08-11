using IdentityServer4.EntityFramework.Entities;
using ByLearning.Admin.EntityFramework.Repository.Context;
using ByLearning.Domain.Core.Exceptions;
using ByLearning.EntityFrameworkCore.MigrationHelper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ByLearning.Admin.EntityFramework.Repository.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task CheckDatabases(IServiceProvider serviceProvider, JpDatabaseOptions options)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var id4Context = scope.ServiceProvider.GetRequiredService<IdentityServer4AdminUiContext>();

            await DbHealthChecker.TestConnection(id4Context);
            await ValidateIs4Context(options, id4Context);
        }

        private static async Task ValidateIs4Context(JpDatabaseOptions options, IdentityServer4AdminUiContext id4AdminUiContext)
        {
            var configurationDatabaseExist = await DbHealthChecker.CheckTableExists<Client>(id4AdminUiContext);
            var operationalDatabaseExist = await DbHealthChecker.CheckTableExists<PersistedGrant>(id4AdminUiContext);
            var isDatabaseExist = configurationDatabaseExist && operationalDatabaseExist;

            if (!isDatabaseExist && options.MustThrowExceptionIfDatabaseDontExist)
                throw new DatabaseNotFoundException("IdentityServer4 Database doesn't exist. Ensure it was created before.'");
        }
    }
}
