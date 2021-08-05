using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Settings;

namespace Acme.BookStore.Settings
{
    public class BookStoreSettingValueProvider : SettingValueProvider
    {
        public override string Name => "Diy";

        public BookStoreSettingValueProvider(ISettingStore settingStore) : base(settingStore)
        {

        }

        public override Task<string> GetOrNullAsync(SettingDefinition setting)
        {
            throw new NotImplementedException();
        }

        public override Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
        {
            throw new NotImplementedException();
        }

    }
}
