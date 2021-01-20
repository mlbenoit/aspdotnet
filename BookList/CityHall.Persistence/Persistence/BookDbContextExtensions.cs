using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookRental.Persistence.Persistence
{
    public static class BookDbContextExtensions
    {
        public static bool AreAllMigrationsApplied(this DbContext context)
        {
            IEnumerable<string> applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            IEnumerable<string> total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static async Task EnsureSeedData(this BookDbContext context)
        {
            bool areAllMigrationsApplied = context.AreAllMigrationsApplied();
            if (!areAllMigrationsApplied)
            {
                return;
            }

            DateTime utcNow = DateTime.UtcNow;
            //await SeedMovementTypes(context, utcNow);
            //await SeedGenders(context, utcNow);
            //await SeedTransactionTypes(context, utcNow);
            //await SeedMonetaryTypes(context, utcNow);
            //await SeedChequeStatuses(context, utcNow);
            //await SeedBeneficiaryTypes(context, utcNow);
            //await SeedTransactionStatuses(context, utcNow);
        }
    }
}
