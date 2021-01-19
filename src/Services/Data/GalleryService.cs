using Common.Helpers;
using Data.Contracts.Repositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Data;
using Services.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Data
{
    public class GalleryService : IGalleryService
    {
        private readonly IDeletableEntityRepository<Gallery> repository;
        private readonly IDeletableEntityRepository<Image> imageRepository;

        public GalleryService(IDeletableEntityRepository<Gallery> repository, IDeletableEntityRepository<Image> imageRepository)
        {
            this.repository = repository;
            this.imageRepository = imageRepository;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.repository.AllAsNoTracking().
               OrderBy(x => x.CreatedOn).
               ThenBy(x => x.Title).
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
        public async Task<Gallery> CreateAsync(string title, IEnumerable<Image> images, string preDesc, string desc)
        {
            var gallery = new Gallery
            {
                Title=title,
                Images= images,
                PreDescription = preDesc,
                Description = desc.SanitizeHtml(),
            };

            await this.repository.AddAsync(gallery);
            await this.repository.SaveChangesAsync();

            return gallery;
        }

        public async Task UpdateAsync(string id, string title, IEnumerable<Image> images, string preDesc, string desc)
        {
            var gallery = new Gallery
            {
                Id = id,
                Title = title,
                Images = images,
                PreDescription = preDesc,
                Description = desc.SanitizeHtml(),
            };

            foreach (var image in images)
            {
                if(string.IsNullOrWhiteSpace(image.GalleryId))
                {
                    image.GalleryId = id;
                    image.Description = title;
                    image.Note = title;
                    this.imageRepository.Update(image);
                }
            }

            this.repository.Update(gallery);
            await this.imageRepository.SaveChangesAsync();
            await this.repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var gallery = await this.repository.GetByIdWithDeletedAsync(id);
            if (gallery == null)
            {
                return false;
            }

            this.repository.Delete(gallery);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task AddGaleriesToSitemapAsync()
        {
            var galleries = await this.repository.AllAsNoTracking().ToListAsync();
            foreach (var gallery in galleries)
            {
                SitemapFactory.AppendSitemapNode(
                    UrlGenerator.GenerateGalleryUrl(gallery.Id, gallery.Title.GenerateSlug()),
                    gallery.ModifiedOn ?? gallery.CreatedOn
                    );
            }
        }
    }
}
