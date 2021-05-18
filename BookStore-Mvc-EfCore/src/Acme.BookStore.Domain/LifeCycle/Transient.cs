//  ************************************************************************************
//  项目名称：Acme.BookStore.Domain
//  文 件  名：Transient.cs
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
    /// 瞬时生命周期
    /// 这个生命周期是用完就销毁
    /// </summary>
    public class Transient : IRunnable, ITransientDependency
    {
        public IRepository<Book, Guid> books { get; set; }

        public Singleton Singleton { get; set; }
        public Scoped Scoped { get; set; }
        public Task Start()
        {
            Console.WriteLine("Start：Transient");
            return Task.CompletedTask;

        }

        public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Console.WriteLine("StartAsync：Transient");
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}