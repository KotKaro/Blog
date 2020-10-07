using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Blog.Application.Mappers;
using Blog.DataAccess;
using Blog.Domain.DataAccess;
using Blog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Module = Autofac.Module;

namespace Blog.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Module
    {
        private readonly IConfiguration _configuration;

        public ApplicationModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder containerBuilder)
        {
            RegisterUnitOfWork(containerBuilder);
            RegisterSqlConnection(containerBuilder, _configuration.GetConnectionString("SqlDatabase"));

            containerBuilder.RegisterModule(new AuthModule());
            containerBuilder.RegisterModule(new RepositoryModule());

            RegisterAutoMapper(containerBuilder);
            RegisterDbContext<BlogDbContext>(containerBuilder);
        }

        private static void RegisterAutoMapper(ContainerBuilder containerBuilder)
        {
            var assembly = Assembly.GetAssembly(typeof(PostToPostDtoMapper));

            containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => t.BaseType == typeof(Profile)
                            && !t.IsAbstract && t.IsPublic)
                .As<Profile>();

            containerBuilder.Register(ctx => new MapperConfiguration(cfg =>
            {
                foreach (var profile in ctx.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            }));

            containerBuilder.Register(ctx =>
            {
                var mapperConfiguration = ctx.Resolve<MapperConfiguration>();
                return mapperConfiguration.CreateMapper();
            })
                .As<IMapper>()
                .SingleInstance();
        }

        private void RegisterSqlConnection(ContainerBuilder containerBuilder, string connectionString)
        {
            containerBuilder.Register(_ => new MySqlConnection(connectionString))
                .As<IDbConnection>()
                .As<MySqlConnection>()
                .InstancePerLifetimeScope();
        }

        private void RegisterUnitOfWork(ContainerBuilder containerBuilder)
        {
            var dbContextType = typeof(BlogDbContext);

            containerBuilder.Register(context => new EfUnitOfWork(
                    context.Resolve(dbContextType) as DbContext,
                    context.Resolve<IMediator>()
                ))
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        }

        private void RegisterDbContext<TDbContext>(ContainerBuilder containerBuilder) where TDbContext : DbContext
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContextPool<TDbContext>((provider, opts) =>
            {
                opts.UseMySql(provider.GetService<MySqlConnection>(), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name);
                    sqlOptions.MigrationsHistoryTable("__EFMigrationHistory");
                    sqlOptions.ServerVersion(new Version(8, 0, 21), ServerType.MySql);
                    sqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend);
                });
            });

            containerBuilder.Populate(serviceCollection);
        }
    }
}
