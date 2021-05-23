using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;

using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.Settings;

namespace Acme.BookStore.UseCache
{
    /// <summary>
    /// 使用ABP缓存
    /// Package： Volo.Abp.Caching 
    /// </summary>
    public class UseDistributedCache : DomainService
    {
        public void Start()
        {
            UseDistributedCache1();
            UseDistributedCache2();
            UseDistributedCache3();
        }

        /// <summary>
        /// 它适用于 byte 数组 而不是 .NET 对象. 因此你需要对缓存的对象进行序列化/反序列化.
        /// 它为所有的缓存项提供了 单个 key 池, 因此;
        /// 你需要注意键区分 不同类型的对象.
        /// 你需要注意不同租户(参见多租户)的缓存项.
        /// </summary>
        public IDistributedCache DistributedCache { get; set; }
        public void UseDistributedCache1()
        {
            var temp = JsonSerializer.Serialize(new CacheData { Name = "A" });
            var arr = Encoding.UTF8.GetBytes(temp);

            DistributedCache.Set("key", arr);
            var valueBytes = DistributedCache.Get("key");
            var byteArr = Encoding.UTF8.GetString(valueBytes);
            var value1 = JsonSerializer.Deserialize<CacheData>(byteArr);
            DistributedCache.Set("key", Encoding.UTF8.GetBytes("B"));
            var value2 = Encoding.UTF8.GetString(DistributedCache.Get("key"));
        }

        /// <summary>
        /// 它在内部 序列化/反序列化 缓存对象. 默认使用 JSON 序列化, 但可以替换依赖注入系统中 IDistributedCacheSerializer 服务的实现来覆盖默认的处理.
        /// 它根据缓存中对象类型自动向缓存key添加 缓存名称 前缀.默认缓存名是缓存对象类的全名
        /// (如果你的类名以CacheItem 结尾, 那么CacheItem 会被忽略, 不应用到缓存名称上). 你也可以在缓存类上使用 CacheName 设置换缓存的名称.
        /// 它自动将当前的租户id添加到缓存键中, 以区分不同租户的缓存项(只有在你的应用程序是多租户的情况下生效). 在缓存类上应用 IgnoreMultiTenancy attribute, 可以在所有的租户间共享缓存.
        /// 允许为每个应用程序定义 全局缓存键前缀, 不同的应用程序可以在共享的分布式缓存中拥有自己的隔离池.
        /// </summary>
        public IDistributedCache<CacheData> DsCache { get; set; }
        public void UseDistributedCache2()
        {

            DsCache.Set("key", new CacheData { Name = "A" });
            var value1 = DsCache.Get("key");
            var a = DateAndTime.Now;
            Thread.Sleep(new TimeSpan(0, 0, 10));
            var b = DateAndTime.Now;
            //DsCache.Set("key", new CacheData { Name = "B" });
            var value2 = DsCache.Get("key");

        }

        /// <summary>
        /// 可以自定义Key类型
        /// </summary>
        public IDistributedCache<CacheData, int> Cache { get; set; }

        public void UseDistributedCache3()
        {
            List<CacheData> dataSource = new List<CacheData>();
            for (var i = 0; i < 10; i++)
            {
                dataSource.Add(new CacheData
                {
                    Id = i,
                    Name = "A"+i
                });
            }

            var keys = dataSource.Select(t => t.Id).ToList();
            var values = dataSource.Select(t => t.Name).ToList();
            var dic = dataSource.ToDictionary(x => x.Id, x => x.Name);
            

            Cache.Set(1, new CacheData { Name = "A" });
            var value1 = Cache.Get(1);

            Cache.Set(1, new CacheData { Name = "B" });
            var value2 = Cache.Get(1);

        }
    }
}