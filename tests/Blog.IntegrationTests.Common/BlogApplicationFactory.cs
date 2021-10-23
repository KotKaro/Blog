using System.Collections.Generic;
using Autofac.Extensions.DependencyInjection;
using Blog.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blog.IntegrationTests.Common
{
    public class BlogApplicationFactory : WebApplicationFactory<Startup>
    {
        private const string TestConnectionString =
            "server=localhost;Port={0};Uid=karol;password=toor;database=blog;";

        protected override IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(
                    logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                    });

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var mySqlContainer = new DockerContainerStarter()
                .GetMysqlContainer()
                .GetAwaiter()
                .GetResult();
            
            builder
                .UseStartup<Startup>()
                .UseEnvironment("Development")
                .ConfigureAppConfiguration(configuration =>
                {
                    var connectionString = string.Format(TestConnectionString, mySqlContainer.Item2);
                    var memoryConfigurationSource = new MemoryConfigurationSource();
                    memoryConfigurationSource.InitialData = new[]
                    {
                        new KeyValuePair<string, string>("ConnectionStrings:SqlDatabase", connectionString)
                    };

                    configuration.Add(memoryConfigurationSource);
                });
        }
    }
}