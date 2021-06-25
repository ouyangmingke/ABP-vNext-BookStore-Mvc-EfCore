using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Products
{
    public class ProductDto : AuditedEntityDto<int>, IMultiTenant
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
    }
}
