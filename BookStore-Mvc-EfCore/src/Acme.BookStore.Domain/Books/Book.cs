using System;
using System.ComponentModel.DataAnnotations.Schema;

using Acme.BookStore.Authors;

using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Books
{
    /// <summary>
    /// Book实体继承了AuditedAggregateRoot,
    /// 聚合根并且增加了审计
    /// AuditedAggregateRoot类在AggregateRoot类的基础上添加了一些审计属性
    /// (CreationTime, CreatorId, LastModificationTime 等).
    /// ABP框架自动为你管理这些属性.
    /// Guid是Book实体的主键类型.
    /// </summary>
    public partial class Book : AuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 将实体的必填属性作为构造函数参数,这样可以创建一个有效(符合规则)的实体.
        /// 另外,将非必填属性作为构造函数的可选参数.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="authorId"></param>
        /// <param name="price"></param>
        /// <param name="publishDate"></param>
        public Book(Guid authorId, string name, BookType type, DateTime publishDate, float price)
        {
            Type = type;
            Name = Check.NotNullOrWhiteSpace(Name, nameof(Name)); // 参数必须检查有效性.
            AuthorId = authorId;
            Price = price;
            PublishDate = publishDate;
        }
        // 不允许定义属性导航关联 遵循可序列化原则 避免不同聚合之间相互操作
        // 想要获取关联聚合根 应该使用 ID 在数据库进行一次查询
        //[ForeignKey(nameof(AuthorId))]
        //public Author Author { get; set; }

        /// <summary>
        /// 两个聚合根只使用 ID 进行连接
        /// </summary>
        public Guid AuthorId { get; set; }

        public string Name { get; set; }

        public BookType Type { get; private set; }

        /// <summary>
        /// 不允许修改发布日期
        /// </summary>
        public DateTime PublishDate { get; private set; }

        public float Price { get; set; }
    }
}
