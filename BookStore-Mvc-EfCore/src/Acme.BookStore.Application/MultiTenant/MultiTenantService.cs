using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Clients;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.MultiTenant
{
    /// <summary>
    /// 多租户服务
    /// </summary>
    public class MultiTenantService : IMultiTenantService
    {
        /// <summary>
        /// 获取当前租户
        /// Volo.Abp.AspNetCore.MultiTenancy
        /// 实现了从当前Web请求(从子域名,请求头,cookie,路由...等)
        /// 获取当前租户信息
        /// </summary>
        public ICurrentTenant CurrentTenant { get; set; }

        /// <summary>
        /// 获取当前租户
        /// </summary>
        public void GetTenant()
        {
            var tenantId = CurrentTenant.Id;// 租户的唯一Id
             var name = CurrentTenant.Name;// 租户的唯一名称
            var isAvailable = CurrentTenant.IsAvailable;
        }
    }
}
