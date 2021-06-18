using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Acme.BookStore.BackgroundWorker
{
    /// <summary>
    /// 异步后台工作者
    /// </summary>
    public class MyAsyncPeriodicBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public MyAsyncPeriodicBackgroundWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
        {
            timer.Period = 1000;
        }

        private int _Count = 0;
        protected override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            _Count++;
            return Task.CompletedTask;
        }
    }
}
