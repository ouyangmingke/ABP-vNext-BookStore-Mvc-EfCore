using Acme.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Acme.BookStore.ATest
{
    /* Inherit your application services from this class.
     */
    public class ATestAppService : BookStoreAppService
    {
        [Authorize(BookStorePermissions.MyPermission1)]
        public string NeedAuthorityMyPermission1()
        {
            return BookStorePermissions.MyPermission1;
        }
        [Authorize(BookStorePermissions.MyPermission10)]
        public string NeedAuthorityMyPermission10()
        {
            return BookStorePermissions.MyPermission10;
        }
    }
}
