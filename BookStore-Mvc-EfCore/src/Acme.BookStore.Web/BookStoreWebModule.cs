using System;
using System.Collections.Generic;
using System.IO;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Acme.BookStore.EntityFrameworkCore;
using Acme.BookStore.Localization;
using Acme.BookStore.MultiTenancy;
using Acme.BookStore.Permissions;
using Acme.BookStore.Web.Menus;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Acme.BookStore.Web
{
    [DependsOn(
        typeof(BookStoreHttpApiModule),
        typeof(BookStoreApplicationModule),
        typeof(BookStoreEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpTenantManagementWebModule),
        typeof(AbpAspNetCoreSerilogModule)
        )]
    public class BookStoreWebModule : AbpModule
    {
        /// <summary>
        /// 预配置
        /// </summary>
        /// <param name="context"></param>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(BookStoreResource),
                    typeof(BookStoreDomainModule).Assembly,
                    typeof(BookStoreDomainSharedModule).Assembly,
                    typeof(BookStoreApplicationModule).Assembly,
                    typeof(BookStoreApplicationContractsModule).Assembly,
                    typeof(BookStoreWebModule).Assembly
                );
            });
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureUrls(configuration);
            ConfigureAuthentication(context, configuration);
            ConfigureAutoMapper();
            ConfigureVirtualFileSystem(hostingEnvironment);
            ConfigureLocalizationServices();
            ConfigureNavigationServices();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);

            // 获取配置并使用  RazorPagesOptions 的可以在预配置中修改
            // 没有权限的用户将被重定向到登录页面
            //Configure<RazorPagesOptions>(options =>
            //{
            //    options.Conventions.AuthorizePage("/Books/Index", BookStorePermissions.Books.Default);
            //    options.Conventions.AuthorizePage("/Books/CreateModal", BookStorePermissions.Books.Create);
            //    options.Conventions.AuthorizePage("/Books/EditModal", BookStorePermissions.Books.Edit);
            //});
        }

        /// <summary>
        /// 配置Url
        /// </summary>
        /// <param name="configuration"></param>
        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        /// <summary>
        /// 配置身份验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            // 开启身份验证
            context.Services.AddAuthentication()
                // 添加JwtBearer服务
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "BookStore";
                });
        }

        /// <summary>
        /// 配置数据映射
        /// </summary>
        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BookStoreWebModule>();
            });
        }

        private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<BookStoreDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Acme.BookStore.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BookStoreDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Acme.BookStore.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BookStoreApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Acme.BookStore.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BookStoreApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}Acme.BookStore.Application"));
                    options.FileSets.ReplaceEmbeddedByPhysical<BookStoreWebModule>(hostingEnvironment.ContentRootPath);
                });
            }
        }

        private void ConfigureLocalizationServices()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<BookStoreResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );

                options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });
        }

        private void ConfigureNavigationServices()
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new BookStoreMenuContributor());
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(BookStoreApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    // 这里可以创建多个 Swagger 文档
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "BookStore API",
                        Version = "v1",
                        Description = "BookStore 接口文档说明",
                        TermsOfService = new Uri("http://tempuri.org/terms "),// 服务条款
                        Contact = new OpenApiContact()// 联系方式
                        {
                            Name = "GitHub： Swashbuckle.AspNetCore",
                            Email = "XXX@gmail.con",// OpenAPI接口规范文档  https://swagger.io/specification/#oasObject
                            Url = new Uri("https://github.com/domaindrivendev/Swashbuckle.AspNetCore")
                        },
                        License = new OpenApiLicense()// 许可证信息
                        {
                            Name = "许可证： Apache 2.0",
                            Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
                        }
                    });

                    // 自定义文档选择 更具文档名称和文档描述  进行选择   这里未筛选
                    options.DocInclusionPredicate((docName, description) => true);

                    // 避免命名空间不同但是名称相同  使用全名进行区分
                    options.CustomSchemaIds(type => type.FullName);

                    #region 添加安全定义和要求
                    // 定义安全方案
                    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                    {
                        Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",// 方案描述
                        Name = "Authorization",// token 名称
                        In = ParameterLocation.Header,// token 所在位置
                        Type = SecuritySchemeType.ApiKey,// 方案类型
                        BearerFormat = "JWT",// 提示客户端识别无记名令牌是如何格式化
                        Scheme = "Bearer"// 使用 HTTP 授权模式的名称  RFC 7235: Authentication 协议

                    });

                    // 指明方案适用于哪些操作  全局应用方案
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "ApiKey" // 指定方案
                                }
                            },
                            new string[] { }// 应用非“oauth2”类型的方案时，范围数组必须为空。
                        }
                    });
                    #endregion


                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();// 使用身份验证中间件
            app.UseJwtTokenMiddleware();// 使用Jwt令牌

            if (MultiTenancyConsts.IsEnabled)
            {
                // 多租户中间件
                app.UseMultiTenancy();
            }

            app.UseAbpRequestLocalization();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
