
using Data.Models;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface ISettingsService
    {
        public Task UpdateAsync(string id, bool homePageVisible, bool homePageGalleryVisible, string homePageTitle, string homePageContent, string homePreContent, bool homeGalleryPreTextVisible, bool homeGalleryPostTextVisible);
        public Task<T> GetSettingAsync<T>();
        public Task<bool> DeleteSettingAsync(string id);
    }
}
