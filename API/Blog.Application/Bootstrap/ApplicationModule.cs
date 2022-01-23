using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Blog.Application.Behaviors;
using Blog.Application.Mappers;
using Blog.Application.Queries.GetPosts;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace Blog.Application.Bootstrap
{
    public class ApplicationModule : Module
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
            
            builder.RegisterAutoMapper(typeof(PostToPostDtoMapper).Assembly);
        }
    }
}
