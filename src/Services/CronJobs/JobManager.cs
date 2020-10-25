using Data.Models;
using Hangfire;

namespace Services.CronJobs
{
    public static class JobManager
    {
        public static void SeedHangfireJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate<MakeVisibleVideosJob>($"UpdateVideoVisibility", x => x.Work(null), "*/2 * * * *");
        }

        public static void StartSubscriptionEmailJob(string htmlText)
        {
            BackgroundJob.Enqueue<SendSubscribersEmailJob>(x => x.Work(htmlText,null));
        }

        public static void StartSubscriptionEmailJob(Video video)
        {
            BackgroundJob.Enqueue<SendSubscribersEmailJob>(x => x.Work(video, null));
        }

        public static void SendContactLetterJob(ContactLetter letter)
        {
            BackgroundJob.Enqueue<SendAdminsContactEmailJob>(x => x.Work(letter, null));
        }
    }
}
