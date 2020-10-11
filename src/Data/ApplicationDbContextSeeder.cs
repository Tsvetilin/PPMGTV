using Data.Contracts.Seeders;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class ApplicationDbContextSeeder : IApplicationSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var seeders = from type in this.GetType().Assembly.GetTypes()
                          where typeof(ISeeder).IsAssignableFrom(type) && type.IsClass
                          select (ISeeder)Activator.CreateInstance(type);

            seeders = seeders.OrderBy(x => x.Priority);

            logger.LogInformation("Application seeder initialized.");

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }

            logger.LogInformation("Application seeding completed.");
        }
    }
}
