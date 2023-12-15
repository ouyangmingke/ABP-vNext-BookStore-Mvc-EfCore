using System;
using System.Collections.Generic;

using Acme.BookStore.BackgroundWorker;
using Acme.BookStore.Filter;
using Acme.BookStore.LifeCycle;
using Acme.BookStore.MultiTenant;
using Acme.BookStore.ObjectExtending;
using Acme.BookStore.UseCache;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation;

namespace Acme.BookStore
{
    [DependsOn(
        typeof(BookStoreDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpFeatureManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpIdentityServerDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpPermissionManagementDomainIdentityServerModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpTenantManagementDomainModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpCachingModule)
        )]
    public class BookStoreDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            BookStoreDomainObjectExtensions.Configure();
            //SkipAutoServiceRegistration = true;// 禁用 ABP 自动注册
            AutoAddDataSyncSynchronizerProviders(context.Services);
        }

        /// <summary>
        /// 手动拦截服务
        /// </summary>
        /// <param name="services"></param>
        private static void AutoAddDataSyncSynchronizerProviders(IServiceCollection services)
        {
            var works = new List<Type>();
            services.OnRegistered(context =>
            {
                // public virtual bool IsAssignableFrom (Type? c);
                // c 和当前实例表示相同类型
                // c 是从当前实例直接或间接派生的 当前实例是
                // c 实现的一个接口
                //          c 是一个泛型类型参数，并且当前实例表示 c 的约束之一
                //          泛型参数需要一致
                // c 表示一个值类型，并且当前实例表示 Nullable<c>

                // ImplementationType 提供服务类型
                if (typeof(IWorker).IsAssignableFrom(context.ImplementationType))
                {
                    works.Add(context.ImplementationType);
                }
            });

            services.Configure<WorkerOptions>(option =>
            {
                // 将获取到的 work 添加到 WorkerOptions 中然后 可以使用多个 实现
                foreach (var work in works)
                {
                    option.BackgroundWorkers.Add(work);
                }
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 配置多租户扩展方法
            context.ConfigureMultiTenant();

            // 手动注入 禁用ABP自动注入后
            // context.Services.AddAssemblyOf<Worker>();

            // 配置数据过滤器
            Configure<AbpDataFilterOptions>(options =>
                {
                    // 设置默认禁用 IIsActive 过滤器
                    // 除非使用 IDataFilter.Enable  启用过滤器
                    // 否者在查询数据库时会包含全部实体
                    options.DefaultStates[typeof(IIsActive)] = new DataFilterState(isEnabled: false);
                }
            );

            // 分布式缓存
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
                    SlidingExpiration = new TimeSpan(0, 0, 3),
                    // 到达固定时间后删除
                    // AbsoluteExpiration = new DateTimeOffset(b),
                    // 创建后到达固定时间删除
                    //AbsoluteExpirationRelativeToNow = new TimeSpan(0, 0, 1, 0)
                };
            });

            ConfigureBackgroundServices();

            // 注册一个singleton实例
            // IWorker 已经通道继承 ISingletonDependency 实现依赖注入
            // context.Services.AddSingleton<IWorker,Worker>();

        }

        /// <summary>
        /// 配置后台服务
        /// </summary>
        private void ConfigureBackgroundServices()
        {
            // 后台作业
            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.IsJobExecutionEnabled = false; // false 禁用作业执行
            });

            // 如果你在集群环境中运行同时运行应用程序的多个实现,
            // 这种情况下要小心,每个应用程序都运行相同的后台工作者,
            // 如果你的工作者在相同的资源上运行(例如处理相同的数据),那么可能会产生冲突.
            // 1、 禁用其他的后台工作者系统,只保留一个实例
            // 2、 所有的应用程序都禁用后台工作者系统,创建一个特殊的应用程序在一个服务上运行执行工作者
            Configure<AbpBackgroundWorkerOptions>(options =>
            {
                options.IsEnabled = false; // false 禁用后台工作者
            });
        }

        /// <summary>
        /// 应用初始化之前
        /// </summary>
        /// <param name="context"></param>
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            /***
             *
             * ConfigureAwait 指定参数为false，即使有当前上下文或调度程序用于回调，它也会假装没有
             * 如果await之后的代码并不需要在原始上下文中运行，那么使用ConfigureAwait(false)就可以避免上述花销：
             * 它不用排队，且可以利用所有可以进行的优化，还可以避免不必要的线程静态访问。
             * 提升性能 避免死锁
             */
            context.ServiceProvider.GetService<Singleton>().StartAsync().ConfigureAwait(false);
            context.ServiceProvider.GetService<Scoped>().StartAsync().ConfigureAwait(false);
            context.ServiceProvider.GetService<Transient>().StartAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 程序初始化
        /// </summary>
        /// <param name="context"></param>
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            // 程序初始化时找到 IWorker 服务 使用 Start方法
            //context.ServiceProvider.GetService<IWorker>().Start();
            //context.ServiceProvider.GetService<UseDistributedCache>().Start();
        }

        /// <summary>
        /// 程序初始化之后
        /// </summary>
        /// <param name="context"></param>
        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {

        }
    }
}
