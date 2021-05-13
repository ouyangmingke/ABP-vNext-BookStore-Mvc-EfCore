using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Acme.BookStore.EntityFrameworkCore
{
    [DependsOn(
        typeof(BookStoreEntityFrameworkCoreModule)
        )]
    public class BookStoreEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BookStoreMigrationsDbContext>();
            // 如果想修改ABP 数据架构 使用该方式
            //context.Services.Configure<SettingManagementModelBuilderConfigurationOptions>(options =>
            //{
            //    options.TablePrefix = "";
            //});

        }
    }
}
