﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Acme.BookStore.Authors;
using Acme.BookStore.Permissions;

using Microsoft.AspNetCore.Authorization;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Books
{
    /// <summary>
    /// 注入图书实体应用服务
    /// 该服务使用了 BookStore.Books 权限分组
    /// </summary>
    [Authorize(BookStorePermissions.Books.Default)]
    public class BookAppService :
        CrudAppService<// CURD基类
            Book, //The Book entity  实体
            BookDto, //Used to show books 服务层使用Dto
            Guid, //Primary key of the book entity 主键
            PagedAndSortedResultRequestDto, //Used for paging/sorting 分页排序
            CreateUpdateBookDto>, //Used to create/update a book 创建更新Dto
        IBookAppService //implement the IBookAppService 实现接口
    {
        /// <summary>
        /// Author存储库
        /// </summary>
        private readonly IAuthorRepository _authorRepository;

        public BookAppService(
            IRepository<Book, Guid> repository,
            IAuthorRepository authorRepository)
            : base(repository)
        {

            _authorRepository = authorRepository;

            // 配置权限 CrudAppService 的基类 AbstractKeyReadOnlyAppService 会自动对CRUD操作使用这些权限

            GetPolicyName = BookStorePermissions.Books.Default;
            GetListPolicyName = BookStorePermissions.Books.Default;
            CreatePolicyName = BookStorePermissions.Books.Create;
            UpdatePolicyName = BookStorePermissions.Books.Edit;
            DeletePolicyName = BookStorePermissions.Books.Create;
        }

        public override async Task<BookDto> GetAsync(Guid id)
        {
            //Prepare a query to join books and authors
            var query = from book in Repository
                        join author in _authorRepository on book.AuthorId equals author.Id
                        where book.Id == id
                        select new { book, author };

            //Execute the query and get the book with author
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Book), id);
            }

            var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
            bookDto.AuthorName = queryResult.author.Name;
            return bookDto;
        }

        public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            //Prepare a query to join books and authors
            var query = from book in Repository
                        join author in _authorRepository on book.AuthorId equals author.Id
                        orderby input.Sorting //TODO: Can not sort like that!
                        select new { book, author };

            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            //Execute the query and get a list
            var queryResult = await AsyncExecuter.ToListAsync(query);

            //Convert the query result to a list of BookDto objects
            var bookDtos = queryResult.Select(x =>
            {
                var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
                bookDto.AuthorName = x.author.Name;
                return bookDto;
            }).ToList();

            //Get the total count with another query
            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<BookDto>(
                totalCount,
                bookDtos
            );
        }

        public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
        {
            var authors = await _authorRepository.GetListAsync();

            return new ListResultDto<AuthorLookupDto>(
                ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors)
            );
        }
    }
}
