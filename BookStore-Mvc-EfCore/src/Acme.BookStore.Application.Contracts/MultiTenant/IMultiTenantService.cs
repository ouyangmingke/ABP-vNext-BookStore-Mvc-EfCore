using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.MultiTenant
{

    /// <summary>
    /// 多租户服务
    /// </summary>
    public interface IMultiTenantService
    {
        void GetTenant();
    }
}
