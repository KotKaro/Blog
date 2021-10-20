using System.Linq;
using Blog.Auth.Abstractions;
using Blog.Domain.Models.Aggregates.User;
using Blog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Blog.API.Common
{
    public static class HostExtensions
    {
        public static IHost ApplyMigrations(this IHost host)
        {
            if (host.Services.GetService(typeof(BlogDbContext)) is not BlogDbContext context)
            {
                return host;
            }

            var appliedMigrationsCount = context.Database.GetAppliedMigrations().Count();
            var availableMigrationsCount = context.Database.GetMigrations().Count();

            if (appliedMigrationsCount != availableMigrationsCount)
            {
                context.Database.Migrate();
            }

            return host;
        }

        public static IHost CreateUserFromConfiguration(this IHost host)
        {
            if (host.Services.GetService(typeof(BlogDbContext)) is not BlogDbContext context)
            {
                return host;
            }

            if (host.Services.GetService(typeof(IPasswordHasher)) is not IPasswordHasher passwordHasher)
            {
                return host;
            }

            var configuration = (IConfiguration)host.Services.GetService(typeof(IConfiguration));

            var username = configuration.GetUsername();
            var password = configuration.GetPassword();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return host;
            }

            if (context.Set<User>().Any(user => user.UserDetails.Username == username))
            {
                return host;
            }

            context.Set<User>().Add(
                new User(
                    new UserDetails(
                        username,
                        passwordHasher.Hash(password)
                    )
                )
            );

            context.SaveChanges();
            return host;
        }
    }
}