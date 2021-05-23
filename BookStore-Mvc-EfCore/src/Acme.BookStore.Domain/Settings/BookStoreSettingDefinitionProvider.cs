using Volo.Abp.Settings;

namespace Acme.BookStore.Settings
{
    /// <summary>
    /// 定义设置
    /// 不同的模块有不同的设置
    /// </summary>
    public class BookStoreSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(BookStoreSettings.MySetting1));
            var a = context.GetAll();
            var b = context.GetOrNull("Abp.Mailing.Smtp.Host");

            context.Add(new SettingDefinition(BookStoreSettings.CurrentEnvironment, "DEV"));
            context.Add(new SettingDefinition("sett1", "s1"));
            context.Add(new SettingDefinition("sett2", "s2"));
        }
    }
}
