﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Acme.BookStore.Products;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.MultiTenant
{
    /// <summary>
    /// 多租户域服务
    /// </summary>
    public class MultiTenantService : DomainService
    {
        /// <summary>
        /// 数据仓储
        /// </summary>
        public IRepository<Product, int> ProductRepository { get; set; }

        /// <summary>
        /// 数据过滤器
        /// </summary>
        public  IDataFilter DataFilter { get; set; }

        /// <summary>
        /// 创建一个新的产品
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public async Task<Product> CreateAsync(string name, float price)
        {
            // CurrentTenant DomainService 已经预先注入

            var product = new Product(name, price, CurrentTenant.Id);
            return await ProductRepository.InsertAsync(product);
        }

        /// <summary>
        /// 获取当前租户产品总数
        /// </summary>
        /// <param name="tenantId">指定租户</param>
        /// <returns></returns>
        public async Task<long> GetProductCountAsync(Guid? tenantId)
        {
            // 使用using 可以在代码块结束后恢复原本租户值
            using (CurrentTenant.Change(tenantId))
            {
                return await ProductRepository.GetCountAsync();
            }
        }

        /// <summary>
        /// 获取全部产品总数
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetProductCountAsync()
        {
            // 使用数据过滤器 忽略租户属性
            using (DataFilter.Disable<IMultiTenant>())
            {
                return await ProductRepository.GetCountAsync();
            }
        }
    }
}
