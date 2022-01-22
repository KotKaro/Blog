using Autofac;
using Blog.API.Behaviors;
using Blog.Application.Queries.GetPosts;
using MediatR.Extensions.Autofac.DependencyInjection;
using Module = Autofac.Module;

namespace Blog.API.Infrastructure.AutofacModules
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMediatR(typeof(GetPostsQuery).Assembly, typeof(TransactionBehaviour<,>).Assembly);
            builder.RegisterGeneric(typeof(TransactionBehaviour<,>))
                .AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>))
                .AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(LoggingBehaviour<,>))
                .AsImplementedInterfaces();
        }
    }
}
