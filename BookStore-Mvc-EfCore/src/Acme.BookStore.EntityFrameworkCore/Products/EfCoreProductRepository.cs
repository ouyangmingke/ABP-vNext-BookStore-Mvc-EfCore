using Acme.BookStore.EntityFrameworkCore;

using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.Products
{
    public class EfCoreProductRepository : EfCoreRepository<ProductDbContext, Product, int>, IProductRepository
    {
        public EfCoreProductRepository(IDbContextProvider<ProductDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
