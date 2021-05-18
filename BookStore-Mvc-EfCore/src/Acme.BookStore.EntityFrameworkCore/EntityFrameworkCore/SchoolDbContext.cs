using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Acme.BookStore.School;

using Microsoft.EntityFrameworkCore;

using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class SchoolDbContext : AbpDbContext<SchoolDbContext>
    {
        public DbSet<Classes> Classes { get; set; }
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }
    }
}
