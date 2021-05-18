using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.School
{
    public interface ISchoolRepository : IRepository<Classes, int>
    {
    }
}
