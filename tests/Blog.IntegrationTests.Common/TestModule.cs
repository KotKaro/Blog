using System;
using System.Linq;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;
using Microsoft.EntityFrameworkCore;
using Module = Autofac.Module;

namespace Blog.IntegrationTests.Common
{
    public class TestModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            base.AttachToComponentRegistration(componentRegistry, registration);

            if (registration.Activator.LimitType.IsSubclassOf(typeof(DbContext)))
            {
                registration.PipelineBuilding += (_, pipeline) =>
                {
                    pipeline.Use(new DbContextMiddleware()); 
                };
            }
        }
    }

    public class DbContextMiddleware : IResolveMiddleware
    {
        public PipelinePhase Phase => PipelinePhase.ParameterSelection;

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            // Continue the resolve.
            next(context);

            // Has an instance been activated?
            if (context.NewInstanceActivated)
            {
                var dbContext = context.Instance as DbContext ?? throw new ArgumentException();

                var appliedMigrationsCount = dbContext.Database
                    .GetAppliedMigrations()
                    .Count();

                var migrationsCount = dbContext.Database
                    .GetMigrations()
                    .Count();

                if (appliedMigrationsCount != migrationsCount)
                {
                    dbContext.Database.Migrate();
                }
            };

        }
    }
}
