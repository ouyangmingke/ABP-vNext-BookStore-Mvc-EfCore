using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Settings;

namespace Acme.BookStore.Settings
{
    /// <summary>
    /// 自定义设置提供程序
    /// </summary>
    public class BookStoreSettingValueProvider : SettingValueProvider
    {
        public override string Name => "Diy";

        public BookStoreSettingValueProvider(ISettingStore settingStore) : base(settingStore)
        {

        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting)
        {
            return Task.FromResult("未实现");
        }

        public override Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
        {
            return Task.FromResult(new List<SettingValue>
            {
                new SettingValue("未","实现")
            });
        }

    }
}
