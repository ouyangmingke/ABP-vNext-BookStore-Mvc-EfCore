using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Products
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}
