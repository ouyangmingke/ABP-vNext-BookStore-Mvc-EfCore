namespace Acme.BookStore.Permissions;

public static class BookStorePermissions
{
    public const string GroupName = "BookStore";

    //Add your own permission names. Example:
    public const string MyPermission1 = GroupName + ".MyPermission1";

    public const string Title = GroupName + ".Title";
    public const string Description = GroupName + ".Description";
    public const string Author = GroupName + ".Author";

    public const string GroupNameZZ = "ZZ";
    public const string ZZMyPermission1 = GroupNameZZ + ".MyPermission1";
    public const string ZZMyPermission2 = GroupNameZZ + ".MyPermission2";
}
