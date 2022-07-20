using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Caching;

namespace Acme.BookStore.UseCache
{
    [Serializable]
    [CacheName("CacheData")]// 自定义缓存名称 默认为 类型全名称
    public class CacheData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
