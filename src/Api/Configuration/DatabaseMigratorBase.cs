using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Configuration
{
    public static class DatabaseMigratorBase
    {
        public static void MigrateDatabase(ApplicationDbContext context)
        {
            if (context.Database.IsInMemory())
            {
                // Skip migrations or handle in-memory database scenario
                return;
            }

            var migrator = context.GetService<IMigrator>();

            migrator.Migrate();
        }
    }
}