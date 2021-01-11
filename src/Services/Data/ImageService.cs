using Common.Enums;
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
    public class ImageService : IImageService
    {
        private readonly IDeletableEntityRepository<Image> repository;

        public ImageService(IDeletableEntityRepository<Image> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.repository.AllAsNoTracking().
               OrderBy(x => x.Category).
               ThenBy(x => x.CreatedOn).
               To<T>().
               ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(string Id)
        {
            return await this.repository.AllAsNoTracking().
               Where(x=>x.Id==Id).
               To<T>().
               FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetByTypeAndNoteAsync<T>(ImageType type, string note=null)
        {
            var all = await this.repository.AllAsNoTracking().ToListAsync();
            var fullMatch = all.Where(x => x.Category == type && x.Note == note).ToList();
            var typeMatch = all.Where(x => x.Category == type).ToList();
            var noteMatch = all.Where(x => x.Note == note);
            fullMatch.AddRange(typeMatch);
            fullMatch.AddRange(noteMatch);
            var result = await fullMatch.Distinct().AsQueryable().To<T>().ToListAsync();
            return result;
        }

        public async Task<Image> CreateAsync( string url, ImageType category, string note, string desc)
        {
            var image = new Image
            {
                Url = url,
                Category = category,
                Note = note,
                Description = desc,
            };

            await this.repository.AddAsync(image);
            await this.repository.SaveChangesAsync();

            return image;
        }

        public async Task UpdateAsync(string id, string url, ImageType category, string note, string desc)
        {
            var image = new Image
            {
                Id=id,
                Url = url,
                Category = category,
                Note = note,
                Description = desc,
            };

            this.repository.Update(image);
            await this.repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var image = await this.repository.GetByIdWithDeletedAsync(id);
            if (image == null)
            {
                return false;
            }

            this.repository.Delete(image);
            await repository.SaveChangesAsync();
            return true;
        }
    }
}
