using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blog.Application.Bootstrap;
using Blog.Infrastructure.Bootstrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module = Autofac.Module;

namespace Blog.API.Infrastructure.AutofacModules
{
    public class ApiModule : Module
    {
        private readonly IConfiguration _configuration;

        public ApiModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(_ => _configuration)
                .AsImplementedInterfaces();
            
            containerBuilder.RegisterModule(new AuthModule());
            containerBuilder.RegisterModule(new InfrastructureModule());
            containerBuilder.RegisterModule(new ApplicationModule());

            AddLogging(containerBuilder);
        }

        private static void AddLogging(ContainerBuilder containerBuilder)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            containerBuilder.Populate(serviceCollection);
        }
    }
}