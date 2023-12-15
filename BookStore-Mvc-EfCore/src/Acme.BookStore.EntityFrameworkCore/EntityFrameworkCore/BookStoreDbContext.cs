using System;
using System.Linq.Expressions;
using System.Reflection;
using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Filter;
using Microsoft.EntityFrameworkCore;
using Acme.BookStore.Users;
using Microsoft.EntityFrameworkCore.Metadata;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
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
            // 目前无法在同一实体上定义多个查询过滤器 - 只会应用最后一个。
            // 但是，可以使用逻辑AND运算符（&&在 C# 中）定义具有多个条件的单个过滤器。


            // 筛选影子属性 IsActive 为true 的 Book
            // builder.Entity<Book>().HasQueryFilter(b => EF.Property<bool>(b, "IsActive") == true);

            // 只要 绑定了 Blog Id 的 Posts 数据
            // 配置一个关系，其中此实体类型具有包含关系中其他类型实例的集合。
            // 这个写法可能会出现问题 如果Blog数据被筛选
            // 使用Include查询Posts相关数据时 EF Core 会使用INNER JOIN在进行查询
            // 将导致过滤器将过滤掉所有与已过滤Blog相关的Post。
            // 解决方案可以是 使用  .IsRequired(false)   或者 禁用过滤器
            // builder.Entity<Blog>().HasMany(b => b.Posts).WithOne(p => p.Blog).IsRequired();

            // 过滤 Book.Name 中不含 H 的数据
            // 使用该方式无法使用 ABP IDataFilter 开启关闭
            // 如果想关闭需要在Dbcontext查询中使用 .IgnoreQueryFilters() 
            //  await dbSet.AsQueryable()//查询数据库
            //  .IgnoreQueryFilters()
            //  .ToListAsync();
            builder.Entity<Book>().HasQueryFilter(t => t.Name.Contains("H"));

            builder.ConfigureBookStore();
        }


        /// <summary>
        /// 活跃数据过滤器是否启用
        /// </summary>
        protected bool IsActiveFilterEnabled => DataFilter?.IsEnabled<IIsActive>() ?? false;

        /// <summary>
        /// ABP EFCore 自定义过滤
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <returns></returns>
        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
        {
            var expression = base.CreateFilterExpression<TEntity>();

            // 检查实体是否拥有 IsActive 属性
            if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> isActiveFilter =
                    e => !IsActiveFilterEnabled || EF.Property<bool>(e, "IsActive");
                expression = expression == null
                    ? isActiveFilter
                    : QueryFilterExpressionHelper.CombineExpressions(expression, isActiveFilter);
            }
            return expression;
        }

        /// <summary>
        /// 实现自定义过滤
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityType"></param>
        /// <returns></returns>
        protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
        {
            // 检查实体是否使用了 IsActive
            if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }
            return base.ShouldFilterEntity<TEntity>(entityType);
        }
    }
}
