using System;

using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    /// <summary>
    /// 用于将书籍数据传输到表示层，以便在UI上显示书籍信息。
    /// 源自AuditedEntityDto<Guid>具有审核属性的，
    /// 与Book定义的实体一样
    /// </summary>
    public class BookDto : AuditedEntityDto<Guid>
    {
        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
    }
}