#  多租户

软件**多重租赁**是指一种软件**架构**，其中**单个实例**的软件在服务器上运行，并提供**多租户**。

租户是一组用户，他们共享对软件实例具有特定权限的公共访问权限。

使用多租户架构，软件应用程序旨在为每个租户提供**实例**的**专用共享，包括其数据**、配置、用户管理、租户个人功能和非功能属性。

多租户与多实例架构形成对比，其中独立的软件实例代表不同的租户运行。

## 主机与租户

典型的 **SaaS** / 多租户应用程序有两个主要方面：      **SaaS（软件即服务）**

- 一个**租客**是出钱加以利用该服务的**SaaS**应用程序的客户。
- **主机**是拥有 **SaaS** 应用程序并管理系统的公司。

###  数据库架构

ABP 框架支持以下所有方法将租户数据存储在数据库中；

- **单一数据库**：所有租户都存储在一个数据库中。
- **每个租户的数据库**：每个租户都有一个单独的专用数据库来存储与该租户相关的数据。
- **混合**：一些租户共享一个数据库，而一些租户可能有自己的数据库。

[租户管理模块](https://docs.abp.io/en/abp/latest/Modules/Tenant-Management)（随启动项目预装）允许您为任何租户设置连接字符串（可选），因此您可以实现任何方法。

## 用法

多租户系统旨在**无缝工作，**并尽可能使您的应用程序代码**多租户不知道**。

###  多租户

`IMultiTenant`为[实体](https://docs.abp.io/en/abp/latest/Entities)实现接口以使它们**准备好多租户**。

实现此接口时，ABP 框架会在您从数据库查询时**自动** [过滤](https://docs.abp.io/en/abp/latest/Data-Filtering)当前租户的实体。因此，您无需`TenantId`在执行查询时手动添加条件。默认情况下，租户无法访问其他租户的数据。



#### 为什么 TenantId 属性可以为空？

`IMultiTenant.TenantId`可以为**空**。当它为空时，表示实体由**主机**方拥有而不是由租户拥有。当您在系统中创建租户和主机端都使用的功能时，它很有用。

例如，`IdentityUser`是由[Identity Module](https://docs.abp.io/en/abp/latest/Modules/Identity)定义的实体。主机和所有租户都有自己的用户。因此，对于主机端，用户将有一段`null` `TenantId`时间租户用户将拥有他们的相关`TenantId`.

> **提示**：如果你的实体是租户特定的，并且在主机端没有任何意义，你可以强制不设置`null`为`TenantId`在实体的构造函数。
>
> 

#### 何时设置 TenantId？

ABP Framework 不会`TenantId`为您设置（由于跨租户操作，`TenantId`在某些情况下ABP 无法知道正确的）。因此，您需要**在创建新的多租户实体时自行设置**。

##### 最佳实践

建议`TenantId`**在构造函数中设置**并且不允许再次更改它。因此，`Product`该类可以重写如下：

```c#
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace MultiTenancyDemo.Products
{
    public class Product : AggregateRoot<Guid>, IMultiTenant
    {
        //Private setter prevents changing it later
        public Guid? TenantId { get; private set; }

        public string Name { get; set; }

        public float Price { get; set; }

        protected Product()
        {
            //This parameterless constructor is needed for ORMs
        }
        
        public Product(string name, float price, Guid? tenantId)
        {
            Name = name;
            Price = price;
            TenantId = tenantId; //Set in the constructor
        }
    }
}

```

> 通常使用的`ICurrentTenant`设置`TenantId`，同时创建一个新的`Product`。

### 当前租户

`ICurrentTenant` 是与多租户基础设施交互的主要服务。

> `ApplicationService`，`DomainService`，`AbpController`和一些其他基类已经已经预先注入`CurrentTenant`性质。对于其他类型的类，您可以将 注入`ICurrentTenant`到您的服务中。



#### 租户属性

`ICurrentTenant` 定义以下属性；

- `Id`( `Guid`): 当前租户的 ID。可`null`如果当前用户是主机用户或承租人不能从请求决定。
- `Name`( `string`)：当前租户的姓名。可`null`如果当前用户是主机用户或承租人不能从请求决定。
- `IsAvailable`( `bool`):`true`如果`Id`不是，则返回`null`。



#### 更改当前租户

ABP 框架基于`ICurrentTenant.Id`. 但是，在某些情况下，您可能希望代表特定租户执行操作，通常是在您处于主机上下文中时。

`ICurrentTenant.Change` 方法在有限的范围内更改当前租户，因此您可以安全地为租户执行操作。



### Volo.Abp.AspNetCore.MultiTenancy

添加 **AbpAspNetCoreMultiTenancyModule** 依赖到你的模块:

```csharp
[DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        //...
    }
```

#### 多租户中间件

Volo.Abp.AspNetCore.MultiTenancy包含了多租户中间件...

```csharp
app.UseMultiTenancy();
```





**修改租户并获取数据**

```c#
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace MultiTenancyDemo.Products
{
    public class ProductManager : DomainService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductManager(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<long> GetProductCountAsync(Guid? tenantId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await _productRepository.GetCountAsync();
            }
        }
    }
}

```

### 数据过滤：禁用多租户过滤器

ABP 框架使用[数据过滤](https://docs.abp.io/en/abp/latest/Data-Filtering)系统处理租户之间的数据隔离。在某些情况下，您可能希望禁用它并对所有数据执行查询，而不过滤当前租户。

**示例：获取数据库中的产品数量，包括所有租户的所有产品。**

```c#
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace MultiTenancyDemo.Products
{
    public class ProductManager : DomainService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IDataFilter _dataFilter;

        public ProductManager(
            IRepository<Product, Guid> productRepository,
            IDataFilter dataFilter)
        {
            _productRepository = productRepository;
            _dataFilter = dataFilter;
        }

        public async Task<long> GetProductCountAsync()
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                return await _productRepository.GetCountAsync();
            }
        }
    }
}


```

> 请注意，如果您的租户具有**单独的数据库**，则此方法将不起作用，因为在单个数据库查询中没有从多个数据库进行查询的内置方法。如果你需要它，你应该自己处理。



## 基础设施

### 确定当前租户

多租户应用程序的第一件事是在运行时确定当前租户。

ABP 框架为此提供了一个可扩展的**租户解析**系统。然后在**多租户中间件中**使用租户解析系统来确定当前 HTTP 请求的当前租户。

#### 租户解析器

##### 默认租户解析程序

#### **从Web请求中确定当前租户**

Volo.Abp.AspNetCore.MultiTenancy 添加了下面这些租户解析器,从当前Web请求(按优先级排序)中确定当前租户.

默认情况下提供并配置了以下解析器；

- `CurrentUserTenantResolveContributor`：如果当前用户已登录，则从当前用户的声明中获取租户 ID。**这应该始终是安全性的第一个贡献者**。
- `QueryStringTenantResolveContributor`：尝试从查询**字符串参数**中查找当前租户 ID。参数名称是`__tenant`默认的。
- `FormTenantResolveContributor`：尝试从**表单参数**中查找当前租户 ID。参数名称是`__tenant`默认的。
- `RouteTenantResolveContributor`：尝试从**路由（URL 路径）**中查找当前租户 ID。变量名是`__tenant`默认的。如果你用这个变量定义了一个路由，那么它可以从路由中确定当前的租户。
- `HeaderTenantResolveContributor`：尝试从 **HTTP 标头**中查找当前租户 ID。标头名称是`__tenant`默认的。
- `CookieTenantResolveContributor`：尝试从 **cookie 值**中查找当前租户 ID。cookie 名称是`__tenant`默认的。

###### NGINX 的问题

`__tenant`如果您使用[nginx](https://www.nginx.com/)作为反向代理服务器，则 HTTP 标头中的可能会出现问题。因为它不允许在 HTTP 标头中使用下划线和其他一些特殊字符，您可能需要手动配置它。请参阅以下文件：

http://nginx.org/en/docs/http/ngx_http_core_module.html#ignore_invalid_headers

http://nginx.org/en/docs/http/ngx_http_core_module.html#underscores_in_headers

###### AbpAspNetCoreMultiTenancyOptions

```
__tenant`可以使用 更改参数名称`AbpAspNetCoreMultiTenancyOptions
```

```csharp
services.Configure<AbpAspNetCoreMultiTenancyOptions>(options =>
{
    options.TenantKey = "MyTenantKey";
});
```

> 但是，我们不建议更改此值，因为某些客户端可能会将 假定`__tenant`为参数名称，然后他们可能需要手动配置。

##### 域/子域租户解析器

在实际应用程序中，大多数时候您希望通过子域（如 mytenant1.mydomain.com）或整个域（如 mytenant.com）来确定当前租户。如果是这样，您可以配置`AbpTenantResolveOptions`添加域租户解析器。

```c#
Configure<AbpTenantResolveOptions>(options =>
{
    options.AddDomainTenantResolver("{0}.mydomain.com");
});

```

- `{0}` 是确定当前租户唯一名称的占位符。
- 将此代码添加到[模块](https://docs.abp.io/en/abp/latest/Module-Development-Basics)的`ConfigureServices`方法中。
- 这应该在*Web/API 层*完成，因为 URL 是与 Web 相关的东西。

> 有一个使用子域来确定当前租户的[示例](https://github.com/abpframework/abp-samples/tree/master/DomainTenantResolver)。



##### 自定义租户解析程序

```csharp
 public class MyCustomTenantResolveContributor : ITenantResolveContributor
    {
        public override Task ResolveAsync(ITenantResolveContext context)
        {
            context.TenantIdOrName = ... //从其他地方获取租户id或租户名字...
        }
    }
```



#### [租户存储](https://docs.abp.io/zh-Hans/abp/latest/Multi-Tenancy#%E7%A7%9F%E6%88%B7%E5%AD%98%E5%82%A8)

Volo.Abp.MultiTenancy中定义了 **ITenantStore** 从框架中抽象数据源.你可以实现ITenantStore,让它跟任何存储你租户的数据源(例如关系型数据库)一起工作.



##### 配置数据存储

有一个内置的(默认的)租户存储,叫ConfigurationTenantStore.它可以被用于存储租户,通过标准的[配置系统](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)(使用[Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration)).因此,你可以通过硬编码或者在appsettings.json文件中定义租户.

具体查看官方文档  [配置数据存储](https://docs.abp.io/zh-Hans/abp/latest/Multi-Tenancy#%E7%A7%9F%E6%88%B7%E5%AD%98%E5%82%A8)

**示例：在 appsettings.json 中定义租户**

```json
"Tenants": [
    {
      "Id": "446a5211-3d72-4339-9adc-845151f8ada0",
      "Name": "tenant1"
    },
    {
      "Id": "25388015-ef1c-4355-9c18-f6b6ddbaf89d",
      "Name": "tenant2",
      "ConnectionStrings": {
        "Default": "...tenant2's db connection string here..."
      }
    }
  ]

```

> 建议**使用租户管理模块**，该**模块**在您使用 ABP 启动模板创建新应用程序时已预先配置。



###  其他多租户基础设施

ABP 框架旨在在各个方面尊重多租户，并且大多数情况下一切都会按预期进行。

BLOB 存储、缓存、数据过滤、数据播种、授权和所有其他服务旨在在多租户系统中正常工作。

##  租户管理模块

ABP 框架提供了创建多租户应用程序的所有基础设施，但不对您如何管理（创建、删除...）租户做出任何假设。

该[租户管理模块](https://docs.abp.io/en/abp/latest/Modules/Tenant-Management)提供管理您的租户，并设置其连接字符串一个基本的UI。它是为[应用程序启动模板](https://docs.abp.io/en/abp/latest/Startup-Templates/Application)预先配置的。

**租户管理模块是该`ITenantStore`接口的实现。它将租户存储在数据库中。它还提供 UI 来管理您的租户及其[功能](https://docs.abp.io/en/abp/latest/Features)。**

