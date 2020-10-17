using Common.Constants;
using Data;
using Data.Contracts.Repositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Data;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Data
{
    public class VideosService : IVideosService
    {
        private readonly IDeletableEntityRepository<Video> repository;

        public VideosService(IDeletableEntityRepository<Video> repository)
        {
            this.repository = repository;
        }

        public async Task<T> GetLatestVideoAsync<T>()
        {
            return await repository.All().
                Where(x => x.IsVisible).
                OrderByDescending(x => x.PremiredOn).
                To<T>().
                FirstAsync();
        }

        public double CountAllFilms()
        {
            return repository.All().Count();
        }

        public async Task<IEnumerable<T>> GetVideosOnPageAsync<T>(int currentPage = 1, int videosOnPage = 10)
        {
            return await repository.All().
                Where(x => x.IsVisible).
                OrderByDescending(x => x.PremiredOn).
                Skip((currentPage - 1) * videosOnPage).
                Take(videosOnPage).
                To<T>().
                ToListAsync();
        }

        public async Task<Video> CreateAsync(
            string videoId,
            string title,
            string desc,
            DateTime premiredOn,
            bool isVisible,
            ApplicationUser user)
        {
            var video = new Video
            {
                YouTubeId = videoId,
                VideoUrl = string.Format(UrlTemplates.EmbedVideo, videoId),
                ThumbnailUrl = string.Format(UrlTemplates.ThumbnailVideo, videoId),
                AddedBy = user,
                IsVisible = isVisible,
                Title = title,
                PremiredOn = premiredOn,
                Description = desc,
            };

            await repository.AddAsync(video);
            await repository.SaveChangesAsync();

            return video;
        }

        public async Task<T> GetVideoByIdAsync<T>(string id)
        {
            return await repository.All().Where(x => x.Id == id).To<T>().FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var video = await repository.GetByIdWithDeletedAsync(id);
            if(video == null)
            {
                return false;
            }

            repository.Delete(video);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(
            string id,
            string youtubeId,
            string title,
            string desc,
            DateTime premiredOn,
            bool isVisible,
            ApplicationUser user)
        {
            var video = new Video
            {
                Id = id,
                YouTubeId = youtubeId,
                VideoUrl = string.Format(UrlTemplates.EmbedVideo, youtubeId),
                ThumbnailUrl = string.Format(UrlTemplates.ThumbnailVideo, youtubeId),
                AddedBy = user,
                IsVisible = isVisible,
                Title = title,
                PremiredOn = premiredOn,
                Description = desc,
            };

            repository.Update(video);
            await repository.SaveChangesAsync();
        }
    }
}
