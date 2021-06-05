using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Products
{
    public class Product : AuditedAggregateRoot<int>, IMultiTenant
    {
        /// <summary>
        /// 价格
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// ORM 需要无参构造函数
        /// 创建一个 private 或 protected 构造函数.
        /// 当数据库提供程序从数据库读取你的实体时(反序列化时)将使用它.
        /// </summary>
        protected Product()
        {
        }

        /// <summary>
        /// 在数据初始化时就添加租户
        /// DomainService基类（以及 ABP 框架中的一些常见基类）提供了CurrentTenant
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="tenantId">通常使用的ICurrentTenant设置TenantId，同时创建一个新的Product。</param>
        public Product(string name, float price, Guid? tenantId)
        {
            Name = name;
            Price = price;

            TenantId = tenantId;
        }


    }
}
