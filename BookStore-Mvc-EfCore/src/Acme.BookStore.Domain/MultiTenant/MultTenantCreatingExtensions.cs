using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Acme.BookStore.MultiTenancy;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;

namespace Acme.BookStore.MultiTenant
{
    /// <summary>
    /// 多租户扩展类
    /// </summary>
    public static class MultiTenantCreatingExtensions
    {
        /// <summary>
        /// 配置多租户扩展方法
        /// </summary>
        /// <param name="context"></param>
        public static void ConfigureMultiTenant(this ServiceConfigurationContext context)
        {
            // 启用/禁用多租户
            context.Services.Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

            // 添加自定义租户解析器
            context.Services.Configure<AbpTenantResolveOptions>(options =>
            {
                // 添加子域名解析器
                // 子域名格式: {0}.mydomain.com (作为第二优先级解析器添加, 位于CurrentUserTenantResolveContributor之后)
                // {0}是用来确定当前租户唯一名称的占位符.
                options.TenantResolvers.Insert(1, new DomainTenantResolveContributor("{0}.mydomain.com"));
                // 与上方效果相同
                options.AddDomainTenantResolver("{0}.mydomain.com");
                // 添加全域名解析器 
                options.AddDomainTenantResolver("{0}.com");
                options.TenantResolvers.Add(new MyCustomTenantResolveContributor());
            });

            // 硬编码定义租户
            context.Services.Configure<AbpDefaultTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantConfiguration(
                        id: Guid.Parse("446a5211-3d72-4339-9adc-845151f8ada0"),
                        name: "租户1"
                    ),
                    new TenantConfiguration(
                        id: Guid.Parse("25388015-ef1c-4355-9c18-f6b6ddbaf89d"),
                        name: "租户2"
                    )
                    {
                        //tenant2 设置单独的数据库连接字符串
                        ConnectionStrings =
                        {
                            {ConnectionStrings.DefaultConnectionStringName,""}
                        }
                    }
                };
            });

            // appsettings.json定义租户
            var configuration = BuildConfiguration();
            context.Services.Configure<AbpDefaultTenantStoreOptions>(configuration);

        }

        /// <summary>
        /// 构建配置
        /// 获取json中的配置
        /// </summary>
        /// <returns></returns>
        private static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
