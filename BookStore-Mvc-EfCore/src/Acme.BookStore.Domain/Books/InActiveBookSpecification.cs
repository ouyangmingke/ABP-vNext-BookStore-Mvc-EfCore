using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Specifications;

namespace Acme.BookStore.Books
{
    /// <summary>
    /// 使用规约判断书籍存入时间是否超过一年
    /// 规约是一种强命名,可重用,可组合,可测试的实体过滤器.
    /// 相当于定义一个可复用的 Func类型委托
    /// </summary>
    public class InActiveBookSpecification : Specification<Book>
    {
        public override Expression<Func<Book, bool>> ToExpression()
        {
            var time = DateTime.Now - new TimeSpan(165, 0, 0, 0, 0);
            return book =>
                book.CreationTime < time &&
                           (book.LastModificationTime == null || book.LastModificationTime < time);
        }
    }

    /// <summary>
    /// 使用规约筛选价格 大于 50 元的书籍
    /// </summary>
    public class InPriceSpecification : Specification<Book> {

        public override Expression<Func<Book, bool>> ToExpression() {
            return book => book.Price > 50;
        }
    }
}
