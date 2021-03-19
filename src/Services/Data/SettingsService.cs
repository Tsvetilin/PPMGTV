using Data.Contracts.Repositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Data;
using System.Threading.Tasks;
using Services.Mapping;
using System.Linq;
using Common.Helpers;

namespace Services.Data
{
    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> repository;

        public SettingsService(IDeletableEntityRepository<Setting> repository)
        {
            this.repository = repository;
        }

        /*
        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.repository.AllAsNoTracking().
               OrderBy(x => x.CreatedOn).
               To<T>().
               ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(string Id)
        {
            return await this.repository.AllAsNoTracking().
               Where(x => x.Id == Id).
               To<T>().
               FirstOrDefaultAsync();
        }
        
        public async Task<Setting> CreateAsync(bool homePageVisible, bool homePageGalleryVisible, string homePageTitle, string homePageContent)
        {
            var setting = new Setting
            {
                Id = id,
                IsHomePageNewsVisible=homePageVisible,
                IsHomePageLastGalleryVisible=homePageGalleryVisible,
                HomePageNewsSectionTitle=homePageTitle,
                HomePageNewsContent=homePageContent,
            };

            await this.repository.AddAsync(setting);
            await this.repository.SaveChangesAsync();

            return setting;
        }
        */
        public async Task<T> GetSettingAsync<T>()
        {
            return await this.repository.AllAsNoTracking().
                To<T>().
                FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id, bool homePageVisible, bool homePageGalleryVisible, string homePageTitle, string homePageContent)
        {
            var setting = new Setting
            {
                Id = id,
                IsHomePageNewsVisible=homePageVisible,
                IsHomePageLastGalleryVisible=homePageGalleryVisible,
                HomePageNewsSectionTitle=homePageTitle,
                HomePageNewsContent=homePageContent.SanitizeHtml(),
            };

            this.repository.Update(setting);
            await this.repository.SaveChangesAsync();
        }

        /*
        public async Task<bool> DeleteAsync(string id)
        {
            var setting = await this.repository.GetByIdWithDeletedAsync(id);
            if (setting == null)
            {
                return false;
            }

            this.repository.Delete(setting);
            await repository.SaveChangesAsync();
            return true;
        }*/
    }
}
