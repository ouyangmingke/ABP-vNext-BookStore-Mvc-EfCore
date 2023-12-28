using Acme.BookStore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Acme.BookStore.Permissions;

public class BookStorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BookStorePermissions.GroupName);
        //Define your own permissions here. Example:
        myGroup.AddPermission(BookStorePermissions.MyPermission1, L("Permission:MyPermission1"));
        myGroup.AddPermission(BookStorePermissions.Description, L("Permission:Description"));
        myGroup.AddPermission(BookStorePermissions.Author, L("Permission:Author"));
        myGroup.AddPermission(BookStorePermissions.Title, L("Permission:Title"));

        var myGroupZZ = context.AddGroup(BookStorePermissions.GroupNameZZ);
        myGroupZZ.AddPermission(BookStorePermissions.ZZMyPermission1);
        myGroupZZ.AddPermission(BookStorePermissions.ZZMyPermission2);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BookStoreResource>(name);
    }
}
