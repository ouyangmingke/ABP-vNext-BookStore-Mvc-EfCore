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

using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Products
{
    public class ProductAppService : CrudAppService<Product, ProductDto, int, PagedAndSortedResultRequestDto, ProductDto>, IProductService
    {
        private readonly IRepository<Product, int> _repository;


        public ProductAppService(IRepository<Product, int> repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            //await Ins();
            var a = await base.GetListAsync(input);
            return a;
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var a = await _repository.GetListAsync();

            var b = ObjectMapper.Map<List<Product>, List<ProductDto>>(a);
            return b;
        }
    }
}