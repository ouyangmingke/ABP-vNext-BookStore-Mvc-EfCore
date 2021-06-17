using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Acme.BookStore.BackgroundWorker
{
    /// <summary>
    /// Dependency
    /// 使用属性配置依赖注入
    /// Lifetime: 注册的生命周期:Singleton,Transient或Scoped.
    /// TryRegister: 设置true则只注册以前未注册的服务.使用IServiceCollection的TryAdd ... 扩展方法.
    /// ReplaceServices: 设置true则替换之前已经注册过的服务.使用IServiceCollection的Replace扩展方法.
    /// ExposeServices
    /// 指定注入
    /// 这里只能注入IWorker
    /// </summary>
    [ExposeServices(typeof(IWorker))]
    [Dependency(ServiceLifetime.Singleton, TryRegister = true, ReplaceServices = true)]
    public class Worker : IWorker
    {
        public IBackgroundWorkerManager MyBackgroundWorkerManager { get; set; }

        public IServiceProvider ServiceProvider { get; set; }
        public IServiceScopeFactory ServiceScopeFactory { get; set; }
        public void Start()
        {
            // 通常在 OnApplicationInitialization 添加工作者
            MyBackgroundWorkerManager.Add(ServiceProvider.GetRequiredService<MyBackgroundWorkerBase>());
            MyBackgroundWorkerManager.Add(ServiceProvider.GetRequiredService<MyPeriodicBackgroundWorker>());
            MyBackgroundWorkerManager.Add(ServiceProvider.GetRequiredService<MyAsyncPeriodicBackgroundWorker>());
        }
    }
}
