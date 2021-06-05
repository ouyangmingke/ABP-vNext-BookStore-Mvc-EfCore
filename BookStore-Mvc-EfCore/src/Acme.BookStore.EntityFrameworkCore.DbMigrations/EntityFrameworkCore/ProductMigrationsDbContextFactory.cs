using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Acme.BookStore.EntityFrameworkCore
{
   public class ProductMigrationsDbContextFactory : IDesignTimeDbContextFactory<ProductMigrationsDbContext>
    {
        public ProductMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ProductMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new ProductMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {

            // 细节： 使用 Path.Combine()  适应不同操作系统
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Acme.BookStore.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
