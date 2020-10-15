using Common.Constants;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Data;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Services.Data
{
    public class VideosService : IVideosService
    {
        private readonly ApplicationDbContext context;

        public VideosService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<T> GetLatestVideoAsync<T>()
        {
            return await context.Videos.
                Where(x => x.IsVisible).
                OrderByDescending(x => x.PremiredOn).
                To<T>().
                FirstAsync();
        }

        public double CountAllFilms()
        {
            return context.Videos.Count();
        }

        public async Task<IEnumerable<T>> GetVideosOnPageAsync<T>(int currentPage = 1, int videosOnPage = 10)
        {
            return await context.Videos.
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

            await context.Videos.AddAsync(video);
            await context.SaveChangesAsync();

            return video;
        }

        public async Task<T> GetVideoByIdAsync<T>(string id)
        {
            return await context.Videos.Where(x => x.Id == id).To<T>().FirstOrDefaultAsync();
        }
    }
}
