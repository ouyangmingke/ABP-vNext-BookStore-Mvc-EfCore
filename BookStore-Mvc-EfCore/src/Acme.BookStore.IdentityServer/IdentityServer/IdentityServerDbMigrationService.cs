using Acme.BookStore.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.IdentityServer
{
    public class IdentityServerDbMigrationService : ITransientDependency
    {
        public ILogger<IdentityServerDbMigrationService> Logger { get; set; }

        private readonly IDataSeeder _dataSeeder;
        private readonly IEnumerable<IIdentityServerDbSchemaMigrator> _dbSchemaMigrators;

        public IdentityServerDbMigrationService(
            IDataSeeder dataSeeder,
            IEnumerable<IIdentityServerDbSchemaMigrator> dbSchemaMigrators)
        {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrators = dbSchemaMigrators;

            Logger = NullLogger<IdentityServerDbMigrationService>.Instance;
        }

        public async Task MigrateAsync()
        {
            Logger.LogInformation("Started database migrations...");
            try
            {
                await MigrateDatabaseSchemaAsync();
                await SeedDataAsync();
            }
            catch (System.Exception ex)
            {
                Logger.LogError("Database migrations Error..." + ex.Message);
                throw;
            }
            Logger.LogInformation("Database migrations Success");
        }

        private async Task MigrateDatabaseSchemaAsync()
        {
            foreach (var migrator in _dbSchemaMigrators)
            {
                await migrator.MigrateAsync();
            }
        }

        private async Task SeedDataAsync()
        {
            await _dataSeeder.SeedAsync();
        }
    }
}