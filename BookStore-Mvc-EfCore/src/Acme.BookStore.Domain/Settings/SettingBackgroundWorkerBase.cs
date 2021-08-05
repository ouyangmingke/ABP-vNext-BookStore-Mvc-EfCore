using System.Threading;
using System.Threading.Tasks;

using Volo.Abp.BackgroundWorkers;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace Acme.BookStore.Settings
{
    public class SettingBackgroundWorkerBase : BackgroundWorkerBase
    {
        /// <summary>
        /// 获取和设定设置值
        /// </summary>
        public SettingManager SettingManager { get; set; }

        /// <summary>
        /// 获取指定设置的值或所有设置的值
        /// </summary>
        public SettingProvider SettingProvider { get; set; }

        public override async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // 设置需要在 SettingDefinitionProvider 进行定义
            string userName1 = await SettingProvider.GetOrNullAsync("BookStore.DEV");
            string userName2 = await SettingManager.GetOrNullConfigurationAsync("BookStore.DEV");
            string userName3 = await SettingManager.GetOrNullAsync("BookStore.DEV", "G", "G");

            await SettingManager.SetGlobalAsync("BookStore.DEV", "NODEV");

            string userName = await SettingProvider.GetOrNullAsync("Abp.Mailing.Smtp.Host");
            string a = await SettingProvider.GetOrNullAsync("BookStore.DEV");
            string b = await SettingProvider.GetOrNullAsync("sett1");
            string c = await SettingProvider.GetOrNullAsync("sett2");
            //  string d = await SettingProvider.GetAllAsync();

        }
    }
}