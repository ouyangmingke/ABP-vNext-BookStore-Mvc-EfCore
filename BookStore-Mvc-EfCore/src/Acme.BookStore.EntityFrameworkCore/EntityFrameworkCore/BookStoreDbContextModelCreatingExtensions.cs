using Acme.BookStore.Authors;
using Acme.BookStore.Books;

using Microsoft.EntityFrameworkCore;

using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Acme.BookStore.EntityFrameworkCore
{
    public static class BookStoreDbContextModelCreatingExtensions
    {

        /// <summary>
        /// 这里使用的是C#的扩展方法
        /// </summary>
        public static void ConfigureBookStore(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */
            /* 配置表和实体*/

            builder.Entity<Book>(b =>
            {
                // 配置表名 公用值 应使用常量 避免拼写错误
                // 表名应该使用  b.Metadata.ClrType.Name 获取该内型名字 
                // 这里 abp 建表时添加了 s 
                b.ToTable(BookStoreConsts.DbTablePrefix + b.Metadata.ClrType.Name + "s", BookStoreConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props

                // 配置属性
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);

                b.HasOne<Author>().WithMany().HasForeignKey(x => x.AuthorId).IsRequired();
            });

            builder.Entity<Author>(b =>
            {
                // 
                b.ToTable(BookStoreConsts.DbTablePrefix + "Authors",
                    BookStoreConsts.DbSchema);

                b.ConfigureByConvention();//配置/映射继承的属性,应始终对你的所有的实体使用它.

                b.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(AuthorConsts.MaxNameLength);

                b.HasIndex(x => x.Name);
            });
        }
    }
}
