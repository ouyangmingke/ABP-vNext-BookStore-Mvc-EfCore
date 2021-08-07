using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Acme.BookStore.Products;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace Acme.BookStore.BackgroundWorker
{
    /// <summary>
    /// 定期后台工作者
    /// </summary>
    public class MyPeriodicBackgroundWorker : PeriodicBackgroundWorkerBase
    {
        public MyPeriodicBackgroundWorker(AbpTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
        {
            timer.Period = 1000;
        }

        private int _count = 0;

        /// <summary>
        /// 执行定期任务.
        /// </summary>
        /// <param name="workerContext"></param>
        protected override void DoWork(PeriodicBackgroundWorkerContext workerContext)
        {
            // 最好使用 PeriodicBackgroundWorkerContext 解析依赖 而不是构造函数.
            // 因为 AsyncPeriodicBackgroundWorkerBase 使用 IServiceScope 在你的任务执行结束时会对其 disposed.
            var schoolRepository = workerContext.ServiceProvider.GetService<IProductRepository>();
            _count++;
        }
    }
}
