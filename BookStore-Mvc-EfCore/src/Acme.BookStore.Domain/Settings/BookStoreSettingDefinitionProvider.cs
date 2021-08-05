using Volo.Abp.Settings;

namespace Acme.BookStore.Settings
{
    /// <summary>
    /// 定义设置
    /// 不同的模块有不同的设置
    /// </summary>
    public class BookStoreSettingDefinitionProvider : SettingDefinitionProvider
    {
        /// <summary>
        /// 在使用 Setting 之前需要在这里配置 SettingDefinition
        /// </summary>
        /// <param name="context"></param>
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(BookStoreSettings.MySetting1));
            var a1 = context.GetAll();
            var d1 = context.GetOrNull(BookStoreSettings.CurrentEnvironment);// NULL  还未定义

            context.Add(new SettingDefinition(BookStoreSettings.CurrentEnvironment, "DEV"));
            context.Add(new SettingDefinition("sett1", "s1"));
            context.Add(new SettingDefinition("sett2", "s2"));
            var d2 = context.GetOrNull(BookStoreSettings.CurrentEnvironment);// 可以获取到

            var a2 = context.GetAll();
        }
    }
}
