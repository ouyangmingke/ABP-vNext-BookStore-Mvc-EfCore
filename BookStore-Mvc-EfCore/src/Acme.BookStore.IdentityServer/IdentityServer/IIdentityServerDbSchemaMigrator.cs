using System.Threading.Tasks;

namespace Acme.BookStore.IdentityServer;

public interface IIdentityServerDbSchemaMigrator
{
    Task MigrateAsync();
}
