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
    public class MyPeriodicBackgroundTwo : AsyncPeriodicBackgroundWorkerBase
    {
        public MyPeriodicBackgroundTwo(AbpTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
        {
            timer.Period = 1000;
        }

        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken = default)
        {
            return base.StopAsync(cancellationToken);
        }
        private int _Count = 0;

        protected override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            _Count++;
            return Task.CompletedTask;
        }
    }
}
