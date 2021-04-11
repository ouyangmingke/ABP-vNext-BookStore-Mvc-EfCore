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

        public IBackgroundWorkerManager MyProperty { get; set; }

        public IServiceProvider serviceProvider { get; set; }
        public IServiceScopeFactory serviceScopeFactory { get; set; }
        public void Start()
        {
            MyProperty.Add(serviceProvider.GetRequiredService<MyPeriodicBackground>());
            MyProperty.Add(serviceProvider.GetRequiredService<MyPeriodicBackgroundTwo>());
            //throw new NotImplementedException();
        }
    }
}
