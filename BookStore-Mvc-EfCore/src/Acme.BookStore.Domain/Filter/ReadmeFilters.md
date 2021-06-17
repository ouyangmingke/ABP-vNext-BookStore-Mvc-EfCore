# 全局查询过滤器

全局查询过滤器是应用于元数据模型（通常在 中`OnModelCreating`）中的实体类型的 LINQ 查询谓词。查询谓词是一个布尔表达式，通常传递给 LINQ`Where`查询运算符。EF Core 自动将此类过滤器应用于涉及这些实体类型的任何 LINQ 查询。EF Core 还将它们应用于实体类型，通过使用 Include 或导航属性间接引用。此功能的一些常见应用是：

- **软删除**- 实体类型定义了一个`IsDeleted`属性。
- **多租户**- 实体类型定义了一个`TenantId`属性。



> 目前无法在同一实体上定义多个查询过滤器 - 只会应用最后一个。但是，您可以使用逻辑`AND`运算符（[`&&`在 C# 中](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#conditional-logical-and-operator-)）定义具有多个条件的单个过滤器。

## 导航的使用

可以在定义全局查询过滤器时使用导航。在查询过滤器中使用导航将导致递归应用查询过滤器。

当 EF Core 扩展查询过滤器中使用的导航时，它还将应用在引用实体上定义的查询过滤器。

```C#
modelBuilder.Entity<Blog>().HasMany(b => b.Posts).WithOne(p => p.Blog);
modelBuilder.Entity<Blog>().HasQueryFilter(b => b.Posts.Count > 0);
modelBuilder.Entity<Post>().HasQueryFilter(p => p.Title.Contains("fish"));
```

```sql
SELECT [b].[BlogId], [b].[Name], [b].[Url]
FROM [Blogs] AS [b]
WHERE (
    SELECT COUNT(*)
    FROM [Posts] AS [p]
    WHERE ([p].[Title] LIKE N'%fish%') AND ([b].[BlogId] = [p].[BlogId])) > 0
```

> 目前 EF Core 不检测全局查询过滤器定义中的循环，因此在定义它们时应该小心。如果指定不正确，循环可能会在查询转换期间导致无限循环。



## 使用所需导航访问带有查询过滤器的实体

> **使用必需的导航来访问定义了全局查询过滤器的实体可能会导致意外结果。**

```c#
modelBuilder.Entity<Blog>().HasMany(b => b.Posts).WithOne(p => p.Blog).IsRequired(false);
modelBuilder.Entity<Blog>().HasQueryFilter(b => b.Url.Contains("fish"));
```

这个写法可能会出现问题 如果 **Blog** 数据被筛选
使用Include查询 **Posts** 相关数据时 EF Core 会使用 INNER JOIN 在进行查询
将导致过滤器将过滤掉所有与已过滤 **Blog** 相关的 **Post**。
解决方案可以是 使用  .IsRequired(false)   或者 禁用过滤器



## 禁用过滤器

```c#
blogs = db.Blogs
    .Include(b => b.Posts)
    .IgnoreQueryFilters()
    .ToList();
```



## 限制

全局查询过滤器有以下限制：

- 只能为继承层次结构的根实体类型定义过滤器。

