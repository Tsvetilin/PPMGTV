using Data.Contracts.Seeders;
using Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    public class SettingsSeeder : ISeeder
    {
        public int Priority => 5;

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (dbContext.Settings.Any())
            {
                return;
            }

            var settings = new Setting
            {
                IsHomePageNewsVisible = false,
                IsHomePageLastGalleryVisible = false,
                HomePageNewsSectionTitle = "",
                HomePageNewsContent = "",
                IsHomePageGalleryPreTextVisible = false,
                HomePageNewsPreContent = "",
                IsHomePageGalleryPostTextVisible = false,
                CreatedOn = DateTime.UtcNow,
            };

            await dbContext.Settings.AddAsync(settings);
        }
    }
}
