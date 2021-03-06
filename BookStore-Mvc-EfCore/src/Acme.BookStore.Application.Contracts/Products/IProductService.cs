﻿using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Products
{
    public interface IProductService
        : ICrudAppService< //Defines CRUD methods 定义了CRUD方法
        ProductDto, //Used to show books 服务层使用Dto
        int, //Primary key of the book entity  主键
        PagedAndSortedResultRequestDto, //Used for paging/sorting 分页排序
        ProductDto> //Used to create/update a book 创建更新Dto
    {
    }
}
