using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Products;
using AutoMapper;

namespace Acme.BookStore
{
    public class BookStoreApplicationAutoMapperProfile : Profile
    {

        /// <summary>
        /// 配置 Dto 映射   将Dto与实体联系起来
        /// 定义正确的映射，AutoMapper库可以自动执行此转换
        /// 如果需要自定义映射关系  可以使用 ForMember();
        /// </summary>
        public BookStoreApplicationAutoMapperProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateUpdateBookDto, Book>();

            CreateMap<Author, AuthorDto>();
            CreateMap<Author, AuthorLookupDto>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
