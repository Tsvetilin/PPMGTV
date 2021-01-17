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
               Where(x => x.Id == Id).
               To<T>().
               FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetByTypeAndNoteAsync<T>(ImageType type, string note = null)
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

        public async Task<Image> CreateAsync(string url, ImageType category, string note, string desc)
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
                Id = id,
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

        public async Task<IEnumerable<Image>> CreateImageListAsync(IEnumerable<string> imagesUrls, ImageType category, string note, string desc)
        {
            var images = await this.repository.AllAsNoTracking().ToListAsync();
            var result = new List<Image>();

            foreach (var imageUrl in imagesUrls)
            {
                var current = images.FirstOrDefault(x => x.Url == imageUrl);
                if (current == null)
                {
                    var image = new Image
                    {
                        Url = imageUrl,
                        Category = category,
                        Note = note,
                        Description = desc,
                    };
                    await this.repository.AddAsync(image);
                    result.Add(image);
                }
                else
                {
                    if (current.Category == ImageType.Unknow)
                    {
                        current.Category = ImageType.GalleryImage;
                        current.Note = note;
                        current.Description = desc;
                        this.repository.Update(current);
                    }
                    result.Add(current);
                }
            }

            await this.repository.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<Image>> UpdateImagesListAsync(IEnumerable<string> imagesUrls, ImageType category, string note, string desc, bool overrideImageData = false)
        {
            var images = await this.repository.AllAsNoTracking().ToListAsync();
            var result = new List<Image>();

            foreach (var imageUrl in imagesUrls)
            {
                var current = images.FirstOrDefault(x => x.Url == imageUrl);
                if (current == null)
                {
                    var image = new Image
                    {
                        Url = imageUrl,
                        Category = category,
                        Note = note,
                        Description = desc,
                    };

                    await this.repository.AddAsync(image);
                    result.Add(image);
                }
                else
                {
                    if(current.Category == ImageType.Unknow)
                    {
                        current.Category = ImageType.GalleryImage;
                        current.Note = note;
                        current.Description = desc;
                        this.repository.Update(current);
                    }

                    if ((current.Note != note || current.Description != desc) && overrideImageData)
                    {
                        current.Note = note;
                        current.Description = desc;
                        this.repository.Update(current);
                    }

                    result.Add(current);
                }
            }

            await this.repository.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<string>> CheckOcupationAsync(IEnumerable<string> urls)
        {
            var occupiedUrls = await this.repository.AllAsNoTracking().
                                          Where(x => !string.IsNullOrWhiteSpace(x.GalleryId)).
                                          Select(x=>x.Url).
                                          ToListAsync();

            var occupied = new List<string>();

            foreach (var url in urls)
            {
                if(occupiedUrls.Contains(url))
                {
                    occupied.Add(url);
                }
            }

            return occupied;
        }
    }
}
