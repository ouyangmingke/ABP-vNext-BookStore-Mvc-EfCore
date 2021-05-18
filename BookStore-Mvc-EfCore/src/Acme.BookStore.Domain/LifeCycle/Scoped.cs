//  ************************************************************************************
//  项目名称：Acme.BookStore.Domain
//  文 件  名：Scoped.cs
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
    /// 作用域生命周期
    /// 这个生命周期是伴随着一个实例
    /// 在 Scoped 生命周期中不能 注入 Transient
    /// 因为无法及时释放
    /// </summary>
    public class Scoped : IRunnable,IScopedDependency
    {
        public IRepository<Book, Guid> books { get; set; }

        //public Transient Transient { get; set; }
        public Singleton Singleton { get; set; }

        public Task Start()
        {
            Console.WriteLine("Start：Scoped");
            return Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Console.WriteLine("StartAsync：Scoped");
            return Task.CompletedTask;
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}