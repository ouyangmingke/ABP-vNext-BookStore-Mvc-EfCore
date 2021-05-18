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
    public class SchoolMigrationsDbContext : AbpDbContext<SchoolMigrationsDbContext>
    {
        public DbSet<Classes> Classes { get; set; }
        public SchoolMigrationsDbContext(DbContextOptions<SchoolMigrationsDbContext> options) : base(options)
        {
        }
    }
}
