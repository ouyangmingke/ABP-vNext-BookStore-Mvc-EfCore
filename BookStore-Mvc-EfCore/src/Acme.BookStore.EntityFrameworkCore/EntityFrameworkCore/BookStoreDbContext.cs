using System;
using System.Linq.Expressions;
using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Microsoft.EntityFrameworkCore;
using Acme.BookStore.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;

namespace Acme.BookStore.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See BookStoreMigrationsDbContext for migrations.
     */
    [ConnectionStringName(BookStoreConsts.ConnectionStringName)]
    public class BookStoreDbContext : AbpDbContext<BookStoreDbContext>
    {
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// EF Core将实体和 DbContext 建立关联
        /// </summary>
        public DbSet<Book> Books { get; set; }

        public DbSet<AppUser> Users { get; set; }

        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside BookStoreDbContextModelCreatingExtensions.ConfigureBookStore
         */

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// 运行的时候使用
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */

            builder.Entity<AppUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser

                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                /* Configure mappings for your additional properties
                 * Also see the BookStoreEfCoreEntityExtensionMappings class
                 */
            });

            /* Configure your own tables/entities inside the ConfigureBookStore method */

            // EFCore 全局数据过滤
            // builder.Entity<Book>().HasQueryFilter(t => t.Name == "过滤数据");

            builder.ConfigureBookStore();
        }

        /// <summary>
        /// ABP EFCore 自定义过滤
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
        {
            return base.CreateFilterExpression<TEntity>();
        }
    }
}
