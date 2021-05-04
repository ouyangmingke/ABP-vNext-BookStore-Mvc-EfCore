using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Volo.Abp.BackgroundWorkers;

namespace Acme.BookStore.BackgroundWorker
{
    /// <summary>
    /// 基础的后台工作者
    /// </summary>
    public class MyBackgroundWorkerBase : BackgroundWorkerBase
    {
        public override Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
