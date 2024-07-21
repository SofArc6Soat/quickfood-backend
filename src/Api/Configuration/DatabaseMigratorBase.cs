using Infra.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Configuration
{
    public static class DatabaseMigratorBase
    {
        public static void MigrateDatabase(ApplicationDbContext context)
        {
            var migrator = context.GetService<IMigrator>();

            migrator.Migrate();
        }
    }
}