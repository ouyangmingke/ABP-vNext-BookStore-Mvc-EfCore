using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    /// <summary>
    /// 注入图书实体的默认仓库接口
    /// </summary>
    public interface IBookAppService :
        ICrudAppService< //Defines CRUD methods 定义了CRUD方法
            BookDto, //Used to show books 服务层使用Dto
            Guid, //Primary key of the book entity  主键
            PagedAndSortedResultRequestDto, //Used for paging/sorting 分页排序
            CreateUpdateBookDto> //Used to create/update a book 创建更新Dto
    {
        Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync();
    }
}