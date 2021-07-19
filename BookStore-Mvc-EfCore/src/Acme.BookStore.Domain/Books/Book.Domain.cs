using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.Books
{
	/// <summary>
	/// 业务逻辑
	/// </summary>
	public partial class Book
	{
		/// <summary>
		/// 业务逻辑
		/// 修改图书类别
		/// </summary>
		/// <param name="type"></param>
		public void SetBookType(BookType type)
		{
			this.Type = type;
		}

		/// <summary>
        /// 使用规约判断书籍是否需要更新
        /// </summary>
        /// <returns></returns>
		public bool IsInActive()
		{
			// IsSatisfiedBy方法,在实例对象上应用规约检查,判断是否满足规约的要求
			return new InActiveBookSpecification().IsSatisfiedBy(this);
		}
	}
}
