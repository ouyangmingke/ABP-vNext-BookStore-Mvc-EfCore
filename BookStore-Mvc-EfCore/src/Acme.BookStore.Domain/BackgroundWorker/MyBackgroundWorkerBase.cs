using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Volo.Abp.BackgroundWorkers;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace Acme.BookStore.BackgroundWorker
{
    /// <summary>
    /// 基础的后台工作者
    /// </summary>
    public class MyBackgroundWorkerBase : BackgroundWorkerBase
    {
        public ISettingProvider SettingProvider { get; set; }
        public ISettingManager SettingManager { get; set; }

        /// <summary>
        /// 在应用程序启动时
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 在应用程序关闭时
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
