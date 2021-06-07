using System;
using System.IO;

using Acme.BookStore.EntityFrameworkCore;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Acme.BookStore.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {


            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            /***********
             *  使用Serilog记录日志信息
             *  MinimumLevel 设置Log打印的最低级别
             *  MinimumLevel.Override 覆盖日志信息 （日志源,日志级别） 这个级别之下的不记录
             ***********/


            Log.Logger = new LoggerConfiguration()
                // #if  条件编译  详细信息查看 Attribute项目 https://github.com/ouyangmingke/Attribute
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Volo", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt"))
                .WriteTo.Async(c => c.MSSqlServer(configuration["ConnectionStrings:Default"]
                    , new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true
                    }))
                .CreateLogger();

            try
            {
                Log.Information("Starting web host.");
                var host = CreateHostBuilder(args).Build();

                // 创建一个“作用域”级别的服务实例 用完就丢弃 using
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    BookStoreDbContext bookStoreDbContext = scope.ServiceProvider.GetService<BookStoreDbContext>();

                    // EnsureCreated 方法会检测数据库是否存在，如果不存在，就创建，然后返回 true；
                    // 如果数据库已经存在，不做任何处理并返回 false。
                    var exist = bookStoreDbContext.Database.EnsureCreated();

                    if (exist)
                    {
                        // 写入初始化数据
                    }

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
