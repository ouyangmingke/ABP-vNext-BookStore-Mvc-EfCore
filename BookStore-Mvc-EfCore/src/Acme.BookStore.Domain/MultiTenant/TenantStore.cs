using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.MultiTenant
{
    /// <summary>
    /// 租户存储   租户管理模块 https://docs.abp.io/en/abp/latest/Modules/Tenant-Management
    /// </summary>
    public class TenantStore : ITenantStore
    {
        public Task<TenantConfiguration> FindAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<TenantConfiguration> FindAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public TenantConfiguration Find(string name)
        {
            throw new NotImplementedException();
        }

        public TenantConfiguration Find(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
