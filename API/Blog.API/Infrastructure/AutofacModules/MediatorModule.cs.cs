using System.Reflection;
using Autofac;
using Blog.API.Behaviors;
using Blog.Application.Queries.GetPosts;
using FluentValidation;
using MediatR;
using Module = Autofac.Module;

namespace Blog.API.Infrastructure.AutofacModules
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(GetPostsQuery).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(GetPostsQuery).Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.RegisterAssemblyTypes(typeof(GetPostsQuery).Assembly)
                .Where(type => type.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => componentContext.TryResolve(t, out var o) ? o : null;
            });

            builder.RegisterGeneric(typeof(TransactionBehaviour<,>))
                .As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(ValidatorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));
            
            builder.RegisterGeneric(typeof(LoggingBehaviour<,>))
                .As(typeof(IPipelineBehavior<,>));
        }
    }
}
