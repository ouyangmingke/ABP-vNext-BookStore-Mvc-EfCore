### [领域层](https://docs.abp.io/en/abp/latest/Tutorials/Part-6?UI=MVC&DB=EF)

> 解决方案的领域层. 它主要包含 [实体, 集合根](https://docs.abp.io/zh-Hans/abp/latest/Entities), [领域服务](https://docs.abp.io/en/abp/latest/Domain-Services), [值类型](https://docs.abp.io/zh-Hans/abp/latest/Value-Types), [仓储接口](https://docs.abp.io/zh-Hans/abp/latest/Repositories) 和解决方案的其他领域对象.

#### 配置实体   [实体和聚合根](https://docs.abp.io/zh-Hans/abp/latest/Entities)

> 实体是DDD(Domain Driven Design)中核心概念.Eric Evans是这样描述实体的 "一个没有从其属性,而是通过连续性和身份的线索来定义的对象"

- > ABP框架为实体提供了两个基本基类：`AggregateRoot`和`Entity`。**聚合根**是[域驱动的设计](https://docs.abp.io/en/abp/latest/Domain-Driven-Design)概念，可以将其视为直接查询和使用的根实体（有关更多信息，请参见[实体文档](https://docs.abp.io/en/abp/latest/Entities)）
  >
  > 



###  [种子数据](https://docs.abp.io/zh-Hans/abp/latest/Data-Seeding)

**介绍**

> 使用数据库的某些应用程序(或模块),可能需要有一些**初始数据**才能够正常启动和运行. 例如**管理员用户**和角色必须在一开始就可用. 否则你就无法**登录**到应用程序创建新用户和角色.
>
> 数据种子也可用于[测试](https://docs.abp.io/zh-Hans/abp/latest/Testing)的目的,你的自动测试可以假定数据库中有一些可用的初始数据.

**ABP框架种子数据系统:**

> - **模块化**: 任何[模块](https://docs.abp.io/zh-Hans/abp/latest/Module-Development-Basics)都可以无声地参与数据播种过程,而不相互了解和影响. 通过这种方式模块将种子化自己的初始数据.
> - **数据库独立**: 它不仅适用于 EF Core, 也使用其他数据库提供程序(如 [MongoDB](https://docs.abp.io/zh-Hans/abp/latest/MongoDB)).
> - **生产准备**: 它解决了生产环境中的问题. 参见下面的*On Production*部分.
> - **依赖注入**: 它充分利用了依赖项注入,你可以在播种初始数据时使用任何内部或外部服务. 实际上你可以做的不仅仅是数据播种.

**使用方法**

> **Acme.BookStore.Domain => DataSeeding.cs**

> - `IDataSeedContributor` 定义了 `SeedAsync` 方法用于执行 **数据种子逻辑**.
> - 通常**检查数据库**是否已经存在种子数据.
> - 你可以**注入**服务,检查数据播种所需的任何逻辑.

> 数据种子贡献者由ABP框架自动发现，并作为数据种子过程的一部分执行。





#### [基类和接口的审计属性](https://docs.abp.io/zh-Hans/abp/latest/Entities#%E5%9F%BA%E7%B1%BB%E5%92%8C%E6%8E%A5%E5%8F%A3%E7%9A%84%E5%AE%A1%E8%AE%A1%E5%B1%9E%E6%80%A7)

> ABP框架提供了一些接口和基类来**标准化**这些属性,并**自动设置它们的值**.

- ##### 审计接口

  > 虽然可以手动实现这些接口,但是可以使用下一节中定义的**基类**简化代码.

- #####  审计基类

  > 使用基类可以简化代码
  
  



##  ABP域服务基础结构

域服务是简单的无状态类。虽然您不必从任何服务或接口派生，但ABP Framework提供了一些有用的基类和约定。

# [ ABP领域层 - 领域服务 书栈网](https://www.bookstack.cn/read/abp/Markdown-Abp-3.4ABP%E9%A2%86%E5%9F%9F%E5%B1%82-%E9%A2%86%E5%9F%9F%E6%9C%8D%E5%8A%A1.md)

### 领域服务 DomainService和IDomainService

从`DomainService`基类派生域服务，或直接实现`IDomainService`接口。

这样做；

- ABP Framework会自动将类注册到具有临时生存期的Dependency Injection系统。
- 您可以直接使用一些常用服务作为基本属性，而无需手动注入（例如[ILogger](https://docs.abp.io/en/abp/latest/Logging)和[IGuidGenerator](https://docs.abp.io/en/abp/latest/Guid-Generation)）。

> 建议使用`Manager`或`Service`后缀来命名域服务。

**领域服务**

> 将该领域逻辑集合起来 外部使用该领域时 只需要调用领域服务即可

###   [域服务](https://docs.abp.io/en/abp/latest/Domain-Services)通常在[应用程序服务中使用](https://docs.abp.io/en/abp/latest/Application-Services)，通常在EntityFrameworkCore中实现

> 注：在**领域驱动设计(DDD)** 中多使用接口进行连接

>  **应用服务与域服务**
>
>  虽然[应用程序服务](https://docs.abp.io/en/abp/latest/Application-Services)和域服务都执行业务规则，但是在逻辑和形式上存在根本的差异。
>
>  - 应用程序服务实现应用程序的**用例**（典型Web应用程序中的用户交互），而域服务实现**核心的，与用例无关的域逻辑**。
>  - 应用程序服务获取/返回[数据传输对象](https://docs.abp.io/en/abp/latest/Data-Transfer-Objects)，域服务方法通常获取和返回**域对象**（[实体](https://docs.abp.io/en/abp/latest/Entities)，[值对象](https://docs.abp.io/en/abp/latest/Value-Objects)）。
>  - 域服务通常由应用程序服务或其他域服务使用，而应用程序服务由表示层或客户端应用程序使用。
>
>  可以这样理解  **域服务** 是对实体操作对数据库进行Crud
>
>  而 **应用程序服务**  是   **基于**    **领域服务** 进行操作的

### 生命周期

> 域服务的生命周期是[短暂的](https://docs.abp.io/en/abp/latest/Dependency-Injection)，它们会自动注册到依赖项注入系统中。