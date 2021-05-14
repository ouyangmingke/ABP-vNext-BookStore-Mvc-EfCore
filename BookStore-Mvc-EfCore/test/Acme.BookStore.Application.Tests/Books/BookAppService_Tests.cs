using System;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;
using Xunit;

namespace Acme.BookStore.Books
{
    public class BookAppService_Tests : BookStoreApplicationTestBase
    {
        private readonly IBookAppService _bookAppService;
        private readonly IAuthorAppService _authorAppService;

        public BookAppService_Tests()
        {
            _bookAppService = GetRequiredService<IBookAppService>();
            _authorAppService = GetRequiredService<IAuthorAppService>();
        }

        /// <summary>
        /// 测试获取一本书
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Get_List_Of_Books()
        {
            //Act 行为： 获取用户列表,并执行检查.
            var result = await _bookAppService.GetListAsync(
                new PagedAndSortedResultRequestDto()
            );

            //Assert  断言： 已确定1984 是存在的  检查 "1984" 这本书的名称 
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(b => b.Name == "1984" && b.AuthorName == "George Orwell");
        }

        /// <summary>
        /// 测试创建一个合法book实体
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Create_A_Valid_Book()
        {
            var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
            var firstAuthor = authors.Items.First();

            //Act 使用正确的方式创建一本书
            var result = await _bookAppService.CreateAsync(
                new CreateUpdateBookDto
                {
                    AuthorId = firstAuthor.Id,
                    Name = "New test book 42",
                    Price = 10,
                    PublishDate = System.DateTime.Now,
                    Type = BookType.ScienceFiction
                }
            );

            //Assert  检查这本书的正确性
            result.Id.ShouldNotBe(Guid.Empty);
            result.Name.ShouldBe("New test book 42");
        }

        /// <summary>
        /// 测试创建一个非法book实体
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Not_Create_A_Book_Without_Name()
        {
            // 由于 Name 是空值, ABP 抛出一个 AbpValidationException 异常.
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _bookAppService.CreateAsync(
                    new CreateUpdateBookDto
                    {
                        Name = "",
                        Price = 10,
                        PublishDate = DateTime.Now,
                        Type = BookType.ScienceFiction
                    }
                );
            });

            exception.ValidationErrors
                .ShouldContain(err => err.MemberNames.Any(m => m == "Name"));
        }
    }
}