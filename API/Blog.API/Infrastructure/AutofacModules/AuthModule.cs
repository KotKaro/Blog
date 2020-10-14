using Autofac;
using Blog.Auth;
using Blog.Auth.Abstractions;
using Blog.Auth.Models;
using Microsoft.Extensions.Options;

namespace Blog.API.Infrastructure.AutofacModules
{
    public class AuthModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                    new PasswordHasher(new OptionsWrapper<HashingOptions>(new HashingOptions { Iterations = 1 })))
                .As<IPasswordHasher>()
                .InstancePerLifetimeScope();

            builder.Register(_ => new JwtContainerModel())
                .As<IAuthContainerModel>()
                .InstancePerLifetimeScope();

            builder.Register(context
                    => new JwtService(context.Resolve<IAuthContainerModel>())
                )
                .As<IJwtService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClaimFactory>()
                .As<IClaimFactory>()
                .InstancePerLifetimeScope();
        }
    }
}
