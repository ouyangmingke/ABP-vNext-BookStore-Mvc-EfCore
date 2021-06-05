using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.BookStore.Products;

using Microsoft.EntityFrameworkCore;

using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class ProductMigrationsDbContext : AbpDbContext<ProductMigrationsDbContext>
    {
        public DbSet<Product> Products { get; set; }
        public ProductMigrationsDbContext(DbContextOptions<ProductMigrationsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 需要使用基类方法
            // 否则迁移时提示
            // The property 'Product.ExtraProperties' could not be mapped because it is of type 'ExtraPropertyDictionary',
            // which is not a supported primitive type or a valid entity type. Either explicitly map this property,
            // or ignore it using the '[NotMapped]' attribute or by using 'EntityTypeBuilder.Ignore' in 'OnModelCreating'.
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureProduct();
        }
    }
}
