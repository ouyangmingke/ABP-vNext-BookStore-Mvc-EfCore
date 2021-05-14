//  ************************************************************************************
//  项目名称：Acme.BookStore.Domain
//  文 件  名：Class1.cs
//  创建时间：2021-05-14
//  作    者：86134
//  说    明：
//  修改时间：2021-05-14
//  修 改 人：86134
//  Copyright © 2020 广州市晨旭自动化设备有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Threading.Tasks;

using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore
{
    /// <summary>
    /// 实体
    /// </summary>
    public class MyEntity : Entity<int>
    {

    }

    /// <summary>
    /// 添加种子数据
    /// </summary>
    public class MyDataSeedContributor : IDataSeedContributor//, ITransientDependency
    {
        public IRepository<MyEntity, int> Repository { get; set; }

        // 执行数据种子逻辑的方法。
        public async Task SeedAsync(DataSeedContext context)
        {

            // 为租户添加数据
            // context.TenantId = Guid.NewGuid();

            // 添加数据前 检查数据库是否已存在种子数据
            if (await Repository.GetCountAsync() > 0)
            {
                return;
            }
            await Repository.InsertAsync(new MyEntity());

        }
    }

    /// <summary>
    /// 手动播种数据
    /// </summary>
    public class MyService : ITransientDependency
    {
        // 用于播种初始数据的主要服务
        private readonly IDataSeeder _dataSeeder;

        public MyService(IDataSeeder dataSeeder)
        {
            _dataSeeder = dataSeeder;
        }

        public async Task FooAsync()
        {
            // 内部调用所有IDataSeedContributor实现以完成数据播种
            // 可以在这里设置租户
            await _dataSeeder.SeedAsync(new Guid());


            // 将命名的配置参数发送到SeedAsync方法
            await _dataSeeder.SeedAsync(
                new DataSeedContext()
                    .WithProperty("MyProperty1", "MyValue1")
                    .WithProperty("MyProperty2", 42)
            );
        }
    }

}