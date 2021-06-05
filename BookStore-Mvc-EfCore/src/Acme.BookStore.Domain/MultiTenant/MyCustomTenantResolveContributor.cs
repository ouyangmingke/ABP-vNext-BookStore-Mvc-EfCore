using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.MultiTenant
{
    /// <summary>
    /// 自定义租户解析器
    /// </summary>
    public class MyCustomTenantResolveContributor : ITenantResolveContributor
    {
        /// <summary>
        /// 自定义租户解析器名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 如果能确定租户id或租户名字可以在租户解析器中设置 TenantIdOrName.
        /// 如果不能确定,那就空着让下一个解析器来确定它.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ResolveAsync(ITenantResolveContext context)
        {
            context.TenantIdOrName = null; //从其他地方获取租户id或租户名字...
            return Task.CompletedTask;
        }

    }
}
