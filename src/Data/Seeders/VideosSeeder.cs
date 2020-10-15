using Data.Contracts.Seeders;
using Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    public class VideosSeeder : ISeeder
    {
        public int Priority => 3;

        public async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (dbContext.Videos.Any())
            {
                return;
            }

            var id = "K-cgrTMcmsQ";
            var video = new Video
            {
                AddedBy = dbContext.Users.First(),
                IsVisible = true,
                PremiredOn = DateTime.UtcNow,
                Title = "Test Video",
                YouTubeId = id,
                VideoUrl = $"https://www.youtube.com/embed/{id}",
                ThumbnailUrl = $"https://img.youtube.com/vi/{id}/hqdefault.jpg",
                Description= "Test video description, describing the video"
            };

            await dbContext.Videos.AddAsync(video);
        }
    }
}
