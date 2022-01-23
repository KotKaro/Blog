using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess;

public static class DbContextExtensions
{
    public static void ApplyMigrations(this DbContext @this)
    {
        var appliedMigrationsCount = @this.Database.GetAppliedMigrations().Count();
        var availableMigrationsCount = @this.Database.GetMigrations().Count();

        if (appliedMigrationsCount != availableMigrationsCount)
        {
            @this.Database.Migrate();
        }
    }
}