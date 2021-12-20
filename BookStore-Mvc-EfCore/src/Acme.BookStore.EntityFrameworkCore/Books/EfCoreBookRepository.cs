using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Acme.BookStore.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace Acme.BookStore.Books
{
    /// <summary>
    /// 实现Domain中自定义的储存库
    /// 从EfCoreRepository继承，因此它继承了标准存储库方法的实现。
    /// 注意这里的与接口的命名不一致  可能导致无法被注入依赖（HTTP请求触发时  其他时候正常  待验证）
    /// </summary>
    public class EfCoreBookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
    {
        public EfCoreBookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public async Task<List<Book>> GetAllListAsync()
        {
            //查询数据库
            //.Include(m => m.Type) 指定相关查询结果 这个用于查询Book 的子集 Type
            //.ThenInclude(m => m.List)// 指定相关结果查询  这个用于查询 Book 的 list 子集
            var dbSet = await GetDbSetAsync();
            return await dbSet.AsQueryable()
                     .ToListAsync();
        }

        public async Task<List<Book>> GetAllIsInActiveAsync(ISpecification<Book> spec)
        {
            return await GetListAsync(spec.ToExpression());
        }
    }
}
