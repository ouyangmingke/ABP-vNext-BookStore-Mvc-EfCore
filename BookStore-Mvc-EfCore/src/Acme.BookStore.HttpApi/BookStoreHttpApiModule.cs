using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.TenantManagement;
using Volo.Abp.SettingManagement;

namespace Acme.BookStore
{
    [DependsOn(
        typeof(BookStoreApplicationContractsModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule)
        )]
    [DependsOn(typeof(AbpSettingManagementHttpApiModule))]
    public class BookStoreHttpApiModule : AbpModule
    {
        
    }
}
