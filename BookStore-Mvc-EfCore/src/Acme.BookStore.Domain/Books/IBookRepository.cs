using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace Acme.BookStore.Books
{
    /// <summary>
    /// 仓储
    /// 只定义聚合对象的仓储
    /// 非聚合对象 通过聚合根进行持久化
    /// 扩展标准IRepository<Book, Guid>接口，减少编写量
    /// </summary>
    public interface IBookRepository : IRepository<Book, Guid>
    {
        Task<List<Book>> GetAllListAsync();

        /// <summary>
        /// 使用规约进行筛选数据
        /// </summary>
        /// <param name="spec">规约</param>
        /// <returns></returns>
        Task<List<Book>> GetAllIsInActiveAsync(ISpecification<Book> spec);

    }
}
