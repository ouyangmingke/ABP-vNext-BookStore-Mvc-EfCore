namespace Acme.BookStore.Permissions
{

    /// <summary>
    /// 定义权限名称
    /// </summary>
    public static class BookStorePermissions
    {
        public const string GroupName = "BookStore";

        public static class Books
        {
            // 分层定义权限名称
            public const string Default = GroupName + ".Books";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Authors
        {
            public const string Default = GroupName + ".Authors";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
