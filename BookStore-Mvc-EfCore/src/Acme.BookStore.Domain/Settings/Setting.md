###  Settings

> 一般使用 appsettings.json 来进行配置 ， ABP提供了另外一种设置和获取应用程序设置的方式.设置存储在动态数据源(通常是数据库)中的键值对. 设置系统预构建了用户,租户,全局和默认设置方法并且可以进行扩展.



### 定义设置  SettingDefinition

`SettingDefinition` 类具有以下属性:

- **Name**: 应用程序中设置的唯一名称. 是**具有约束的唯一属性**, 在应用程序获取/设置此设置的值 (设置名称定义为常量而不是`magic`字符串是个好主意).
- **DefaultValue**: 设置的默认值.
- **DisplayName**: 本地化的字符串,用于在UI上显示名称.
- **Description**: 本地化的字符串,用于在UI上显示描述.
- **IsVisibleToClients**: 布尔值,表示此设置是否在客户端可用. 默认为false,避免意外暴漏内部关键设置.
- **IsInherited**: 布尔值,此设置值是否从其他提供程序继承. 如果没有为请求的提供程序设置设定值,那么默认值是true并回退到下一个提供程序 (参阅设置值提供程序部分了解更多).
- **IsEncrypted**: 布尔值,表示是否在保存值是加密,读取时解密. 在数据库中存储加密的值.
- **Providers**: 限制可用于特定的设置值提供程序(参阅设置值提供程序部分了解更多).
- **Properties**: 设置此值的自定义属性 名称/值 集合,可以在之后的应用程序代码中使用.



### 读取设置 ISettingProvider

```c#
public async Task FooAsync()
    {
        //Get a value as string.
        string userName = await _settingProvider.GetOrNullAsync("Smtp.UserName");

        //Get a bool value and fallback to the default value (false) if not set.
        bool enableSsl = await _settingProvider.GetAsync<bool>("Smtp.EnableSsl");

        //Get a bool value and fallback to the provided default value (true) if not set.
        bool enableSsl = await _settingProvider.GetAsync<bool>(
            "Smtp.EnableSsl", defaultValue: true);

        //Get a bool value with the IsTrueAsync shortcut extension method
        bool enableSsl = await _settingProvider.IsTrueAsync("Smtp.EnableSsl");

        //Get an int value or the default value (0) if not set
        int port = (await _settingProvider.GetAsync<int>("Smtp.Port"));

        //Get an int value or null if not provided
        int? port = (await _settingProvider.GetOrNullAsync("Smtp.Port"))?.To<int>();
    }
```

> 一些基类中(如`IApplicationService`)已经将其属性注入. 这种情况下可以直接使用`SettingProvider`



### 设置值提供程序

> `ISettingProvider` 使用设置值提供程序来获取设置值. 如果值提供程序无法获取设置值,则会回退到下一个值提供程序.

有五个预构建设置值提供程序按以下顺序注册:

- `DefaultValueSettingValueProvider`: 从设置定义的默认值中获取值(参见上面的SettingDefinition部分).
- `ConfigurationSettingValueProvider`: 从[IConfiguration服务](https://docs.abp.io/zh-Hans/abp/latest/Configuration)中获取值.
- `GlobalSettingValueProvider`: 获取设置的全局(系统范围)值.
- `TenantSettingValueProvider`: 获取当前租户的设置值(参阅 [多租户](https://docs.abp.io/zh-Hans/abp/latest/Multi-Tenancy)文档).
- `UserSettingValueProvider`: 获取当前用户的设置值(参阅 [当前用户](https://docs.abp.io/zh-Hans/abp/latest/CurrentUser) 文档).



> `ConfigurationSettingValueProvider` 从 `IConfiguration` 服务中读取设置, 该服务默认从 `appsettings.json` 中读取值

```json
{
  "Settings": {
    "配置名字": "配置值",
  }
}
```



#### 自定义设置值提供程序

扩展设置系统的方式是定义一个派生自 `SettingValueProvider` 的类. 

```c#
public class CustomSettingValueProvider : SettingValueProvider
{
    public override string Name => "Custom";

    public CustomSettingValueProvider(ISettingStore settingStore) 
        : base(settingStore)
    {
    }

    public override Task<string> GetOrNullAsync(SettingDefinition setting)
    {
        /* Return the setting value or null
           Use the SettingStore or another data source */
    }
}
```

> 或者直接可以实现 `ISettingValueProvider` 接口. 这时需要记得将其注册到 [依赖注入](https://docs.abp.io/zh-Hans/abp/latest/Dependency-Injection).

每一个提供程序都应该具有唯一的名称 (这里的名称是 "Custom" ). 内置提供程序使用给定的名称:

**ProviderKey 就是这里的名称**

- `DefaultValueSettingValueProvider`: "**D**".
- `ConfigurationSettingValueProvider`: "**C**".
- `GlobalSettingValueProvider`: "**G**".
- `TenantSettingValueProvider`: "**T**".
- `UserSettingValueProvider`: "**U**".

最好使用一个字母的名称来减少数据库中的数据大小(提供者名称在每行中重复).

定义自定义设置值提供程序后,需要将其显式注册到 `AbpSettingOptions`:

```c#
Configure<AbpSettingOptions>(options =>
{
    options.ValueProviders.Add<CustomSettingValueProvider>();
});
```

> 示例将其添加到最后一项,因此它将成为`ISettingProvider`使用的第一个值提供程序. 也可以将其添加到`options.ValueProviders`列表的另一个位置.



###  ISettingStore

尽管设置值提供程序可以自由使用任何来源来获取设置值,但 `ISettingStore` 服务是设置值的默认来源. 全局,租户和用户设置值提供者都使用它.

##  ISettingEncryptionService

`ISettingEncryptionService` 用于在设置定义的 `isencryption` 属性设置为 `true` 时加密/解密设置值.

你可以在依赖项入系统中替换此服务,自定义实现加密/解密过程. 默认实现 `StringEncryptionService` 使用AES算法(参见字符串[加密文档](https://docs.abp.io/zh-Hans/abp/latest/String-Encryption)学习更多).



##  设置管理模块 Setting Management Module

设置系统核心是相当独立的,不做任何关于如何管理(更改)设置值的假设. 默认的`ISettingStore`实现也是`NullSettingStore`,它为所有设置值返回null.

设置管理模块通过管理数据库中的设置值来完成逻辑(实现`ISettingStore`).有关更多信息参阅[设置管理模块](https://docs.abp.io/zh-Hans/abp/latest/Modules/Setting-Management)学习更多.

##  ISettingManager

```c#
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;

namespace Demo
{
    public class MyService : ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        //Inject ISettingManager service
        public MyService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task FooAsync()
        {
            Guid user1Id = ...;
            Guid tenant1Id = ...;

            //Get/set a setting value for the current user or the specified user
            
            string layoutType1 =
                await _settingManager.GetOrNullForCurrentUserAsync("App.UI.LayoutType");
            string layoutType2 =
                await _settingManager.GetOrNullForUserAsync("App.UI.LayoutType", user1Id);

            await _settingManager.SetForCurrentUserAsync("App.UI.LayoutType", "LeftMenu");
            await _settingManager.SetForUserAsync(user1Id, "App.UI.LayoutType", "LeftMenu");

            //Get/set a setting value for the current tenant or the specified tenant
            
            string layoutType3 =
                await _settingManager.GetOrNullForCurrentTenantAsync("App.UI.LayoutType");
            string layoutType4 =
                await _settingManager.GetOrNullForTenantAsync("App.UI.LayoutType", tenant1Id);
            
            await _settingManager.SetForCurrentTenantAsync("App.UI.LayoutType", "LeftMenu");
            await _settingManager.SetForTenantAsync(tenant1Id, "App.UI.LayoutType", "LeftMenu");

            //Get/set a global and default setting value
            
            string layoutType5 =
                await _settingManager.GetOrNullGlobalAsync("App.UI.LayoutType");
            string layoutType6 =
                await _settingManager.GetOrNullDefaultAsync("App.UI.LayoutType");

            await _settingManager.SetGlobalAsync("App.UI.LayoutType", "TopMenu");
        }
    }
}


```

> 从不同的设置值提供程序中(默认,全局,用户,租户...等)中获取或设定设置值.

> 如果只需要读取设置值,建议使用 `ISettingProvider` 而不是`ISettingManager`,因为它实现了缓存并支持所有部署场景. 如果要创建设置管理UI,可以使用ISettingManager.



###  Setting Cache

设置值缓存在 [分布式缓存](https://docs.abp.io/zh-Hans/abp/latest/Caching) 系统中. 建议始终使用 `ISettingManager` 更改设置值.



## Setting Management Providers

设置管理模块是可扩展的,像[设置系统](https://docs.abp.io/zh-Hans/abp/latest/Settings)一样. 你可以通过自定义设置管理提供程序进行扩展. 有5个预构建的设置管理程序程序按以下顺序注册:

- `DefaultValueSettingManagementProvider`: 从设置定义的默认值中获取值,由于默认值是硬编码在设置定义上的,所以无法更改默认值.
- `ConfigurationSettingManagementProvider`:从 [IConfiguration 服务](https://docs.abp.io/zh-Hans/abp/latest/Configuration)中获取值. 由于无法在运行时更改配置值,所以无法更改配置值.
- `GlobalSettingManagementProvider`: 获取或设定设置的全局 (系统范围)值.
- `TenantSettingManagementProvider`: 获取或设定租户的设置值.
- `UserSettingManagementProvider`: 获取或设定用户的设置值.

`ISettingManager` 在 `get/set` 方法中使用设置管理提供程序. 通常每个设置程序提供程序都在 `ISettingManagement` 服务上定义了模块方法 (比如用户设置管理程序提供定义了 `SetForUserAsync` 方法).