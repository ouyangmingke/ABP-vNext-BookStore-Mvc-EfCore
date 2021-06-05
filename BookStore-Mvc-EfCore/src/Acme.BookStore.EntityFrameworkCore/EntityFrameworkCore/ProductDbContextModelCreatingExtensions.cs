using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Acme.BookStore.Products;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Acme.BookStore.EntityFrameworkCore
{
    public static class ProductDbContextModelCreatingExtensions
    {
        /// <summary>
        /// 扩展方法 好处是在 Migration 中也可以便捷使用
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureProduct(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Product>(b =>
                b.ToTable(BookStoreConsts.DbTablePrefix + b.Metadata.ClrType.Name, BookStoreConsts.DbSchema)
            );
        }
    }
}
