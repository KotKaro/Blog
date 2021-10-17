using System.Linq;
using Autofac.Extensions.DependencyInjection;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Blog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();

            if (host.Services.GetService(typeof(BlogDbContext)) is BlogDbContext context)
            {
                var appliedMigrationsCount = context.Database.GetAppliedMigrations().Count();
                var availableMigrationsCount = context.Database.GetMigrations().Count();

                if (appliedMigrationsCount != availableMigrationsCount)
                {
                    context.Database.Migrate();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
