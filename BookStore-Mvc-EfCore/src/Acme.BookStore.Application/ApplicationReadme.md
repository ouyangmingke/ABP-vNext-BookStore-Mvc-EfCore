#  应用服务  [ABP应用层 - 应用服务 书栈网](https://www.bookstack.cn/read/abp/Markdown-Abp-4.1ABP%E5%BA%94%E7%94%A8%E5%B1%82-%E5%BA%94%E7%94%A8%E6%9C%8D%E5%8A%A1.md)

应用程序服务用于实现应用程序的**用例**。它们用于**将域逻辑公开给表示层**。

使用**DTO（[数据传输对象](https://docs.abp.io/en/abp/latest/Data-Transfer-Objects)）**作为参数从表示层（可选）调用应用程序服务。它使用域对象**执行某些特定的业务逻辑，**并（可选）将DTO返回到表示层。因此，表示层与域层完全**隔离**。

### [AbstractKeyCrudAppService](https://docs.abp.io/en/abp/latest/Application-Services#abstractkeycrudappservice)

`CrudAppService`要求拥有一个Id属性作为您实体的主键。如果您使用的是复合键，那么您将无法使用它。

`AbstractKeyCrudAppService`实现相同的`ICrudAppService`接口，但是这次无需假设您的主键。

### IBookAppService接口  ApplicationService实现

在ABP中，应用程序服务应实现该`IApplicationService`接口。



### 自动生成API Controllers

你通常创建**Controller**以将应用程序服务公开为**HTTP API**端点. 因此允许浏览器或第三方客户端通过AJAX调用它们.

ABP可以[**自动**](https://docs.abp.io/zh-Hans/abp/latest/API/Auto-API-Controllers)按照惯例将你的应用程序服务配置为MVC API控制器.



## 数据传输对象

应用程序服务获取并返回DTO，而不是实体。ABP并不强制执行此规则。但是，将实体暴露于表示层（或远程客户端）存在重大问题，因此不建议这样做。

有关更多信息，请参见[DTO文档](https://docs.abp.io/en/abp/latest/Data-Transfer-Objects)。

### 应用程序层

- 包含您的**应用程序服务**的实现。
- 实现   **Application.Contracts**  编写 Crud接口 的位置

- 注意   **ApplicationAutoMapperProfile**  这个类  


这里进行配置Dto转换

## 对象到对象的映射使用方法

> 通过依赖注入 注入  **IObjectMapper**  然后使用 **Map<>()**  
>
> AutoMapper需要创建一个映射[配置文件类](https://docs.automapper.org/en/stable/Configuration.html#profile-instances)。
>
> ```c#
> public class MyProfile : Profile
> {
>     public MyProfile()
>     {
>         CreateMap<Book, BookDto>();
>     }
> }
> ```
>
> 注册配置文件`AbpAutoMapperOptions`
>
> ```c#
> [DependsOn(typeof(AbpAutoMapperModule))]
> public class MyModule : AbpModule
> {
>     public override void ConfigureServices(ServiceConfigurationContext context)
>     {
>         Configure<AbpAutoMapperOptions>(options =>
>         {
>             //Add all mappings defined in the assembly of the MyModule class
>             options.AddMaps<MyModule>();
>             //AddMaps注册在给定类的程序集中定义的所有概要文件类，通常是您的模块类。它还注册属性映射。
>         });
>     }
> }
> 
> ```
>
> 

```c#
    public class Temp
    {
        public Temp(IObjectMapper objectMapper)
        {
            ObjectMapper = objectMapper;
        }

        private readonly IObjectMapper ObjectMapper;

        public async Task Start()
        {
            SourceType sourceType;
            DestinationType destinationType = ObjectMapper.Map<SourceType, DestinationType>(sourceType);
            await Task.CompletedTask;
        }

    }
```



#### 常见错误

1. > Unmapped members were found. Review the types and members below.
   > Add a custom mapping expression, ignore, add a custom resolver, or modify the source/destination type
   > For no matching constructor, add a no-arg ctor, add optional arguments, or map all of the constructor parameters
   >
   > ***
   >
   > 找到未映射的成员。检查下面的类型和成员。
   >
   > 添加自定义映射表达式、忽略、添加自定义解析器或修改源/目标类型
   >
   > 对于无匹配构造函数，请添加无参数构造函数、添加可选参数或映射所有构造函数参数
   >
   > ***
   >
   > 部分属性无法映射     **需要自定义映射关系**

