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
    /// 同步的后台工作者
    /// </summary>
    public class MyPeriodicBackgroundWorker :PeriodicBackgroundWorkerBase
    {
        public MyPeriodicBackgroundWorker(AbpTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
        {
            timer.Period = 1000;
        }

        private int _count = 0;

        protected override void DoWork(PeriodicBackgroundWorkerContext workerContext)
        {
            _count++;
        }
    }
}
