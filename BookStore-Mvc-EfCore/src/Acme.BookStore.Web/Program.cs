using System;
using System.IO;

using Acme.BookStore.EntityFrameworkCore;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace Acme.BookStore.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {

            /***********
             *  使用Serilog记录日志信息
             *  MinimumLevel 设置Log打印的最低级别
             *  MinimumLevel.Override 覆盖日志信息 （日志源,日志级别） 这个级别之下的不记录
             ***********/


            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine("Config", "serilog.json"))
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Async(t => t.Console())
                .CreateLogger();

            try
            {
                Log.Information("Starting web host.");
                var host = CreateHostBuilder(args).Build();

                // 创建一个“作用域”级别的服务实例 用完就丢弃 using
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    var bookStoreDbContext = scope.ServiceProvider.GetService<BookStoreDbContext>();

                    // EnsureCreated 方法会检测数据库是否存在，如果不存在，就创建，然后返回 true；
                    // 如果数据库已经存在，不做任何处理并返回 false。
                    // 该方式已过时 现在在应用启动前进行数据迁移与播种
                    //var exist = bookStoreDbContext.Database.EnsureCreated();

                    //if (exist)
                    //{
                    //    // 写入初始化数据
                    //}

                }

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog();
    }
}
