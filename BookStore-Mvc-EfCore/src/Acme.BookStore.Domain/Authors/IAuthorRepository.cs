using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Authors
{
    /// <summary>
    /// IAuthorRepository扩展了标准IRepository<Author, Guid>接口，
    /// 因此的所有标准存储库方法也将可用IAuthorRepository。
    /// </summary>
    public interface IAuthorRepository : IRepository<Author, Guid>
    {
        /// <summary>
        /// FindByNameAsync在中用于AuthorManager按名称查询作者。
        /// </summary>
        /// <param name="name">通过名称查询</param>
        /// <returns></returns>
        Task<Author> FindByNameAsync(string name);

        /// <summary>
        /// 分页查询
        /// GetListAsync 将在应用程序层中使用，以获取列出的，
        /// 经过排序和筛选的作者列表，以显示在UI上。
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="sorting"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<Author>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string filter = null
        );
    }
}
