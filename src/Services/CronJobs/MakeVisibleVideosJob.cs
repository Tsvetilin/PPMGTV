using Data.Contracts.Repositories;
using Data.Models;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.CronJobs
{
    public class MakeVisibleVideosJob
    {
        private readonly IDeletableEntityRepository<Video> repository;

        public MakeVisibleVideosJob(IDeletableEntityRepository<Video> repository)
        {
            this.repository = repository;
        }


        [AutomaticRetry(Attempts = 2)]
        public async Task Work(PerformContext context)
        {
            var videos = repository.All();
            if (!videos.Any(x => !x.IsVisible))
            {
                JobManager.StopVideoUpdaterJob();
            }
            foreach (var video in videos)
            {
                if (!video.IsVisible && video.PremiredOn <= DateTime.UtcNow)
                {
                    video.IsVisible = true;
                    JobManager.StartSubscriptionEmailJob(video);
                    context.WriteLine($"Made visible video with YT ID: {video.YouTubeId}, entitled {video.Title}");
                }
            }

            await repository.SaveChangesAsync();
            context.WriteLine("Job finished succesfully, all changes saved.");
        }
    }
}
