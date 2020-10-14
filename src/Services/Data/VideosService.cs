using Data;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Data;
using Services.Mapping;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<T> GetLatestVideo<T>()
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

        public async Task<IEnumerable<T>> GetVideosOnPage<T>(int currentPage = 1, int videosOnPage = 10)
        {
            return await context.Videos.
                Where(x => x.IsVisible).
                OrderByDescending(x => x.PremiredOn).
                Skip((currentPage - 1) * videosOnPage).
                Take(videosOnPage).
                To<T>().
                ToListAsync();
        }
    }
}
