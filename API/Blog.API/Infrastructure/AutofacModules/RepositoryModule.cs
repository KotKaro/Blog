using Autofac;
using Blog.Domain.Repositories;
using Blog.Infrastructure.Repositories;
using Module = Autofac.Module;

namespace Blog.API.Infrastructure.AutofacModules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<PostRepository>()
                .As<IPostRepository>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();
        }
    }
}