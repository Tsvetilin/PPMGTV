using Common.Enums;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface IImageService
    {
        public Task<IEnumerable<T>> GetAllAsync<T>();
        public Task<T> GetByIdAsync<T>(string Id);
        public Task<IEnumerable<T>> GetByTypeAndNoteAsync<T>(ImageType type, string note = null);
        public Task<Image> CreateAsync(string url, ImageType category, string note, string desc);
        public Task UpdateAsync(string id, string url, ImageType category, string note, string desc);
        public Task<bool> DeleteAsync(string id);

        public Task<IEnumerable<Image>> CreateImageListAsync(IEnumerable<string> imagesUrls, ImageType category, string note, string desc);
        public Task<IEnumerable<Image>> UpdateImagesListAsync(IEnumerable<string> imagesUrls, ImageType category, string note, string desc, bool overrideImageData = false);
        public Task<IEnumerable<string>> CheckOcupationAsync(IEnumerable<string> urls, string galleryId);
    }
}
