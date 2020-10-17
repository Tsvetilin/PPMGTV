using Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts.Data
{
    public interface IVideosService
    {
        public Task<T> GetLatestVideoAsync<T>();
        public double CountAllFilms();
        public Task<IEnumerable<T>> GetVideosOnPageAsync<T>(int currentPage, int videosOnPage);
        public Task<Video> CreateAsync(string videoId, string title, string desc, DateTime premiredOn, bool isVisible, ApplicationUser user);
        public Task<T> GetVideoByIdAsync<T>(string id);

        public Task<bool> DeleteAsync(string id);

        public Task UpdateAsync(string id, string youtubeId, string title, string desc, DateTime premiredOn, bool isVisible, ApplicationUser user);
    }
}
