using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Acme.BookStore.EntityFrameworkCore;

using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.School
{
    public class EfCoreSchoolRepository : EfCoreRepository<SchoolDbContext, Classes, int>, ISchoolRepository
    {
        public EfCoreSchoolRepository(IDbContextProvider<SchoolDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
