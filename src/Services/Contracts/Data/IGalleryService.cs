using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface IGalleryService
    {
        public Task<IEnumerable<T>> GetAllAsync<T>();
        public Task<T> GetByIdAsync<T>(string Id);
        public Task<Gallery> CreateAsync(string title, IEnumerable<Image> images, string preDesc, string desc);
        public Task UpdateAsync(string id, string title, IEnumerable<Image> images, string preDesc, string desc);
        public Task<bool> DeleteAsync(string id);

        public Task AddGaleriesToSitemapAsync();
    }
}
