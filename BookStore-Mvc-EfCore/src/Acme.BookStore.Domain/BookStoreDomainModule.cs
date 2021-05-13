using System;

using Acme.BookStore.BackgroundWorker;
using Acme.BookStore.MultiTenancy;
using Acme.BookStore.ObjectExtending;
using Acme.BookStore.UseCache;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

using Volo.Abp;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Caching;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Acme.BookStore
{
    [DependsOn(
        typeof(BookStoreDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AbpCachingModule)
        )]
    public class BookStoreDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            BookStoreDomainObjectExtensions.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
            Configure<AbpDistributedCacheOptions>(options =>
            {
                // 设置key前缀
                options.KeyPrefix = "key";
                // 启用/禁用隐藏从缓存服务器写入/读取值时的错误。
                options.HideErrors = true;
                // 设置缓存储存时间  改配置不能控制 IDistributedCache
                var a = DateTime.Now;
                var b = a + new TimeSpan(0, 0, 5);
                options.GlobalCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    // 多久不访问后删除
                    // SlidingExpiration = new TimeSpan(0,0,3),
                    // 到达固定时间后删除
                    // AbsoluteExpiration = new DateTimeOffset(b),
                    // 创建后到达固定时间删除
                      AbsoluteExpirationRelativeToNow = new TimeSpan(0, 0, 0, 5)
                };
            });

            // 注册一个singleton实例
            // IWorker 已经通道继承 ISingletonDependency 实现依赖注入
            // context.Services.AddSingleton<IWorker,Worker>();

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            // 程序初始化时找到 IWorker 服务 使用 Start方法
            context.ServiceProvider.GetService<IWorker>().Start();
            context.ServiceProvider.GetService<UseDistributedCache>().Start();

        }
    }
}
