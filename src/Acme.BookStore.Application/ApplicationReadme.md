### 应用程序层

- 包含您的应用程序服务的实现。
- 实现   **Application.Contracts**  编写 Crud 的位置

- 注意   **ApplicationAutoMapperProfile**  这个类  


这里进行配置Dto转换

#### 使用方法

> 通过依赖注入 注入  **IObjectMapper**  然后使用 **Map<>()**  
>
> 注意 这里转换的类型 需要在   **Profile**  中配置

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

### 