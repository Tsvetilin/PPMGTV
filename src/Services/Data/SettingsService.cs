using Data.Contracts.Repositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Data;
using System.Threading.Tasks;
using Services.Mapping;
using Common.Extensions;
using System;

namespace Services.Data
{
    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> repository;

        public SettingsService(IDeletableEntityRepository<Setting> repository)
        {
            this.repository = repository;
        }

        public async Task<T> GetSettingAsync<T>()
        {
            return await this.repository.AllAsNoTracking().
                To<T>().
                FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id,
                                      bool homePageVisible,
                                      bool homePageGalleryVisible,
                                      string homePageTitle,
                                      string homePageContent,
                                      string homePreContent,
                                      bool homeGalleryPreTextVisible,
                                      bool homeGalleryPostTextVisible)
        {
            string mainContent = homePageContent.SanitizeHtml(!homePageContent.Contains("video-frame-responsive"));

            var setting = new Setting
            {
                Id = id,
                IsHomePageNewsVisible = homePageVisible,
                IsHomePageLastGalleryVisible = homePageGalleryVisible,
                HomePageNewsSectionTitle = homePageTitle,
                HomePageNewsContent = mainContent,
                HomePageNewsPreContent = homePreContent.SanitizeHtml(),
                IsHomePageGalleryPreTextVisible = homeGalleryPreTextVisible,
                IsHomePageGalleryPostTextVisible = homeGalleryPostTextVisible,
            };

            this.repository.Update(setting);
            await this.repository.SaveChangesAsync();
        }


        public async Task<bool> DeleteSettingAsync(string id)
        {
            var setting = await this.repository.GetByIdWithDeletedAsync(id);
            if (setting == null)
            {
                return false;
            }

            this.repository.Delete(setting);
            await repository.SaveChangesAsync();

            await this.repository.AddAsync(
                new Setting()
                {
                    CreatedOn = DateTime.UtcNow
                });
            await repository.SaveChangesAsync();

            return true;
        }
    }
}
