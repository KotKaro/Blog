using System;
using System.Data;
using System.Data.Common;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blog.DataAccess;
using Blog.Domain.DataAccess;
using Blog.Domain.Repositories;
using Blog.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace Blog.Infrastructure.Bootstrap;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        
        builder.Register(c => new MySqlConnection(c.Resolve<IConfiguration>().GetConnectionString("SqlDatabase")))
            .As<IDbConnection>()
            .As<DbConnection>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<PostRepository>()
            .As<IPostRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<UserRepository>()
            .As<IUserRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<CommentRepository>()
            .As<ICommentRepository>()
            .InstancePerLifetimeScope();
        
        var dbContextType = typeof(BlogDbContext);
        builder.Register(context => new EfUnitOfWork(
                context.Resolve(dbContextType) as DbContext,
                context.Resolve<IMediator>()
            ))
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
        
        RegisterDbContext<BlogDbContext>(builder);
    }
    
    private static void RegisterDbContext<TDbContext>(ContainerBuilder containerBuilder)
        where TDbContext : DbContext
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContextPool<TDbContext>((provider, opts) =>
        {
            opts.UseMySql(
                provider.GetService<DbConnection>()!,
                new MySqlServerVersion(new Version(8, 0, 26)),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name);
                    sqlOptions.MigrationsHistoryTable("__EFMigrationHistory");
                });
        });

        containerBuilder.Populate(serviceCollection);
    }
}