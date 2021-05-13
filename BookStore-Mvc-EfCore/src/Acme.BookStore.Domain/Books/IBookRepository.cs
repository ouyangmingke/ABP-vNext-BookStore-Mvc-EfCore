using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Books
{
    /// <summary>
    /// 扩展标准IRepository<Book, Guid>接口，
    /// </summary>
    public interface IBookRepository : IRepository<Book, Guid>
    {
        Task<List<Book>> GetAllListAsync();
    }
}
