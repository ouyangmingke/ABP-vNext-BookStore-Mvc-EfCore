### [领域层](https://docs.abp.io/en/abp/latest/Tutorials/Part-6?UI=MVC&DB=EF)

- #### 配置实体   [实体和聚合根](https://docs.abp.io/zh-Hans/abp/latest/Entities)

  > 实体是DDD(Domain Driven Design)中核心概念.Eric Evans是这样描述实体的 "一个没有从其属性,而是通过连续性和身份的线索来定义的对象"

  - > ABP框架为实体提供了两个基本基类：`AggregateRoot`和`Entity`。**聚合根**是[域驱动的设计](https://docs.abp.io/en/abp/latest/Domain-Driven-Design)概念，可以将其视为直接查询和使用的根实体（有关更多信息，请参见[实体文档](https://docs.abp.io/en/abp/latest/Entities)）
    >
    > 

  

- #### [基类和接口的审计属性](https://docs.abp.io/zh-Hans/abp/latest/Entities#%E5%9F%BA%E7%B1%BB%E5%92%8C%E6%8E%A5%E5%8F%A3%E7%9A%84%E5%AE%A1%E8%AE%A1%E5%B1%9E%E6%80%A7)

  > ABP框架提供了一些接口和基类来**标准化**这些属性,并**自动设置它们的值**.

  - ##### 审计接口

    > 虽然可以手动实现这些接口,但是可以使用下一节中定义的**基类**简化代码.

  - #####  审计基类

    > 使用基类可以简化代码

- #### [域服务](https://docs.abp.io/en/abp/latest/Domain-Services)通常在[应用程序服务中使用](https://docs.abp.io/en/abp/latest/Application-Services)，通常在EntityFrameworkCore中实现

  > 注：在**领域驱动设计(DDD)** 中多使用接口进行连接

  >  应用服务与域服务
  >
  > 虽然[应用程序服务](https://docs.abp.io/en/abp/latest/Application-Services)和域服务都执行业务规则，但是在逻辑和形式上存在根本的差异。
  >
  > - 应用程序服务实现应用程序的**用例**（典型Web应用程序中的用户交互），而域服务实现**核心的，与用例无关的域逻辑**。
  > - 应用程序服务获取/返回[数据传输对象](https://docs.abp.io/en/abp/latest/Data-Transfer-Objects)，域服务方法通常获取和返回**域对象**（[实体](https://docs.abp.io/en/abp/latest/Entities)，[值对象](https://docs.abp.io/en/abp/latest/Value-Objects)）。
  > - 域服务通常由应用程序服务或其他域服务使用，而应用程序服务由表示层或客户端应用程序使用。
  >
  > 可以这样理解  **域服务** 是对实体操作对数据库进行Crud
  >
  > 而 **应用程序服务**  是   **基于**    **域服务** 进行操作的

### 生命周期

> 域服务的生命周期是[短暂的](https://docs.abp.io/en/abp/latest/Dependency-Injection)，它们会自动注册到依赖项注入系统中。