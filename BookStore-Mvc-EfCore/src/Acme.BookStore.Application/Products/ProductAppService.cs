//  ************************************************************************************
//  项目名称：Acme.BookStore.Application
//  文 件  名：SchoolService.cs
//  创建时间：2021-05-18
//  作    者：
//  说    明：
//  修改时间：2021-05-18
//  修 改 人：
//  Copyright © 2020 广州市晨旭自动化设备有限公司. 版权所有
//  *************************************************************************************

using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Products
{
    /// <summary>
    /// ApiExplorerSettingsAttribute 标记属于 Product 分组
    /// </summary>
    [ApiExplorerSettings(GroupName = "Product")]
    public class ProductAppService : CrudAppService<Product, ProductDto, int,
        PagedAndSortedResultRequestDto, ProductDto>, IProductService
    {
        private readonly IRepository<Product, int> _repository;

        public ProductAppService(IRepository<Product, int> repository) : base(repository)
        {
            _repository = repository;
        }

        public string GetProductsByDbContext()
        {
            // 无法获取DBContext  因为Application 没有依赖 EntityFrameworkCore  无法使用扩展方法
            // 根据数据库独立原则 应用层和领域层 应只通过领域层定义的接口进行数据访问
            // 避免因为切换数据库导致的重写
            return "无需权限";
        }
        /// <summary>
        /// 获取全部产品
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductDto>> GetAllProducts()
        {
            var products = await _repository.GetListAsync();
            return await MapToGetListOutputDtosAsync(products);
        }

        /// <summary>
        /// 需要权限
        /// </summary>
        /// <returns></returns>
        [Authorize(BookStorePermissions.Product.Default)]
        public string CheckDefaultAuthorize()
        {
            return "通过权限";
        }
   
    }
}