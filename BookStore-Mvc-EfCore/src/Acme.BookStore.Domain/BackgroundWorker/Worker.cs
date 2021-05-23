using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Acme.BookStore.BackgroundWorker
{
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
