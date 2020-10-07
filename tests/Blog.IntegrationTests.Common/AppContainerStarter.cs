using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Blog.API.Infrastructure.AutofacModules;
using Microsoft.Extensions.Configuration;

namespace Blog.IntegrationTests.Common
{
    public class AppContainerStarter
    {
        private const string TestConnectionString =
            "server=localhost;Port={0};Uid=karol;password=toor;database=blog;";

        public DockerContainerStarter DockerContainerStarter { get; }

        public AppContainerStarter()
        {
            DockerContainerStarter = new DockerContainerStarter();
        }

        public async Task<IContainer> StartTestContainer()
        {
            var mySqlContainer = await DockerContainerStarter.GetMysqlContainer();

            var configurationBuilder = CreateConfigurationBuilder(mySqlContainer);

            return CreateContainerBuilder(configurationBuilder)
                .Build();
        }

        private static ConfigurationBuilder CreateConfigurationBuilder((string, int) mySqlContainer)
        {
            var configurationBuilder = new ConfigurationBuilder();

            var connectionString = string.Format(TestConnectionString, mySqlContainer.Item2);

            configurationBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("ConnectionStrings:SqlDatabase", connectionString)
            });
            return configurationBuilder;
        }

        private ContainerBuilder CreateContainerBuilder(ConfigurationBuilder configurationBuilder)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new TestModule());
            containerBuilder.RegisterModule(new ApplicationModule(configurationBuilder.Build()));
            return containerBuilder;
        }
    }
}
