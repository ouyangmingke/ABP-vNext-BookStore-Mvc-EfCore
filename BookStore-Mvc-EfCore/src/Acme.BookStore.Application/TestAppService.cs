using IdentityServer4.Stores;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.ObjectMapping;

namespace Acme.BookStore
{
    public class TestAppService : ApplicationService
    {
        /// <summary>
        /// IdentityServer4.AspNetIdentity  获取客户端流程
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<IdentityServer4.Models.Client> GetClientAsync(string clientId)
        {
            // 1、获取客户端信息
            var client = await LazyServiceProvider.LazyGetRequiredService<IClientRepository>().FindByClientIdAsync(clientId);

            // 2、创建客户端配置验证上下文
            var idenclient = LazyServiceProvider.LazyGetRequiredService<IObjectMapper<AbpIdentityServerDomainModule>>()
             .Map<Volo.Abp.IdentityServer.Clients.Client, IdentityServer4.Models.Client>(client);
            var context = new ClientConfigurationValidationContext(idenclient);

            // 3、使用 IdentityServer4 验证器 对客户端配置进行校验，不符合规则的客户端将不可用
            await LazyServiceProvider.LazyGetRequiredService<IClientConfigurationValidator>().ValidateAsync(context);

            // 校验样例
            await ValidateRedirectUriAsync(context);

            return idenclient;
        }

        /// <summary>
        /// 校验重定向 Uri 是否存在
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task ValidateRedirectUriAsync(ClientConfigurationValidationContext context)
        {
            ICollection<string> allowedGrantTypes = context.Client.AllowedGrantTypes;
            if (allowedGrantTypes != null && allowedGrantTypes.Any() && (context.Client.AllowedGrantTypes.Contains("authorization_code") || context.Client.AllowedGrantTypes.Contains("hybrid") || context.Client.AllowedGrantTypes.Contains("implicit")))
            {
                ICollection<string> redirectUris = context.Client.RedirectUris;
                if (redirectUris != null && !redirectUris.Any())
                {
                    context.SetError("No redirect URI configured.");
                }
            }
            return Task.CompletedTask;
        }
    }
}
