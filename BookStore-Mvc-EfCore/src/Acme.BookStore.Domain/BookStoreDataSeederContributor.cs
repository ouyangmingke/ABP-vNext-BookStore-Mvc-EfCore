﻿using System;
using System.Threading.Tasks;

using Acme.BookStore.Authors;
using Acme.BookStore.Books;

using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore
{
    /// <summary>
    /// 添加种子数据
    /// </summary>
    public class BookStoreDataSeederContributor
        : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly AuthorManager _authorManager;

        public BookStoreDataSeederContributor(
            IRepository<Book, Guid> bookRepository,
            IAuthorRepository authorRepository,
            AuthorManager authorManager)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _authorManager = authorManager;
        }

        /// <summary>
        /// 添加种子数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _bookRepository.GetCountAsync() > 0)
            {
                return;
            }

            var orwell = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "George Orwell",
                    new DateTime(1903, 06, 25),
                    "Orwell produced literary criticism and poetry, fiction and polemical journalism; and is best known for the allegorical novella Animal Farm (1945) and the dystopian novel Nineteen Eighty-Four (1949)."
                )
            );

            var douglas = await _authorRepository.InsertAsync(
                await _authorManager.CreateAsync(
                    "Douglas Adams",
                    new DateTime(1952, 03, 11),
                    "Douglas Adams was an English author, screenwriter, essayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
                )
            );

            await _bookRepository.InsertAsync(
                new Book
                (
                  orwell.Id,
                 "1984",
                   BookType.Dystopia,
                    new DateTime(1949, 6, 8),
                    19.84f
                ),
                autoSave: true
            );

            await _bookRepository.InsertAsync(
                new Book
                (
                   douglas.Id,
                 "The Hitchhiker's Guide to the Galaxy",
                 BookType.ScienceFiction,
                   new DateTime(1995, 9, 27),
                   42.0f
                ),
                autoSave: true
            );
        }
    }
}
