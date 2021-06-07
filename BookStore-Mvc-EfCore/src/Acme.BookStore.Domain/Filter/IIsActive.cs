using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.Filter
{
    /// <summary>
    /// 自定义数据过滤器
    /// 比如 ISoftDelete 和 IMultiTenant
    /// </summary>
    public interface IIsActive
    {
        /// <summary>
        /// 过滤活跃/消极数据
        /// </summary>
        bool IsActive { get; }
    }
}
