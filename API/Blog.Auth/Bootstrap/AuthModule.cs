using Autofac;
using Blog.Auth.Abstractions;
using Blog.Auth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Blog.Auth.Bootstrap
{
    public class AuthModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ =>
                    new PasswordHasher(new OptionsWrapper<HashingOptions>(new HashingOptions { Iterations = 1 })))
                .As<IPasswordHasher>()
                .InstancePerLifetimeScope();


            builder.Register(context
                    => new JwtService(context.Resolve<IAuthContainerModel>())
                )
                .As<IJwtService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ClaimFactory>()
                .As<IClaimFactory>()
                .InstancePerLifetimeScope();

            builder.Register(c =>
                {
                    var containerModel = new JwtContainerModel();
                    var key = c.Resolve<IConfiguration>().GetSection("AuthContainer")["SecretKey"];
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        containerModel.SecretKey = key;
                    }

                    return containerModel;
                })
                .As<IAuthContainerModel>()
                .InstancePerLifetimeScope();
        }
    }
}
