//  ************************************************************************************
//  项目名称：Acme.BookStore.Domain
//  文 件  名：Singleton.cs
//  创建时间：2021-05-18
//  作    者：86134
//  说    明：
//  修改时间：2021-05-18
//  修 改 人：86134
//  Copyright © 2020 广州市晨旭自动化设备有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

using Acme.BookStore.Books;

using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace Acme.BookStore.LifeCycle
{

    /// <summary>
    /// 单例生命周期
    /// 在整个应用中是唯一的
    /// 中不能注入 Transient Scoped 生命周期
    /// 因为他们无法被及时释放
    /// </summary>
    public class Singleton : IRunnable, ISingletonDependency
    {
        public IRepository<Book, Guid> books { get; set; }


        //public Transient Transient { get; set; }
        //public Scoped Scoped { get; set; }
        public Task Start()
        {
            Console.WriteLine("Start：Singleton");
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Console.WriteLine("StartAsync：Singleton");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}