using Autofac;
using Blog.Auth;
using Blog.Auth.Models;
using Microsoft.Extensions.Options;

namespace Blog.API.Infrastructure.AutofacModules
{
    public class AuthModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                    new PasswordHasher(new OptionsWrapper<HashingOptions>(new HashingOptions {Iterations = 1})))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.Register(_ => new JwtContainerModel())
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.Register(context
                    => new JwtService(context.Resolve<JwtContainerModel>())
                )
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
