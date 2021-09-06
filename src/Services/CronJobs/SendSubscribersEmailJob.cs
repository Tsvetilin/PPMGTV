using Common.Constants;
using Data.Contracts.Repositories;
using Data.Models;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Services.Contracts.External;
using System.Linq;
using System.Threading.Tasks;

namespace Services.CronJobs
{
    public class SendSubscribersEmailJob
    {
        private readonly IRepository<ApplicationUser> repository;
        private readonly ISendGridEmailSender emailSender;

        public SendSubscribersEmailJob(IRepository<ApplicationUser> repository, ISendGridEmailSender emailSender)
        {
            this.repository = repository;
            this.emailSender = emailSender;
        }

        [AutomaticRetry(Attempts = 2)]
        public async Task Work(string text, PerformContext context)
        {
            var users = repository.All().Where(x => x.IsNewsLetterSubscriber).ToList();
            foreach (var user in users)
            {
                await emailSender.SendEmailAsync(
                    "admin@ppmgtv.com",
                    "PPMGTV NewsLetter",
                    user.Email,
                    "Вижте най-новото от PPMGTV.com",
                    text
                    );
                context.WriteLine($"NewsLetter sent to {user.UserName}.");
            }

            context.WriteLine("Sending newsletters has finished successfully.");
        }

        [AutomaticRetry(Attempts =2)]
        public async Task Work(Video video, PerformContext context)
        {
            var users = repository.All().Where(x => x.IsNewsLetterSubscriber).ToList();
            foreach (var user in users)
            {
                await emailSender.SendEmailAsync(
                    "noreply@ppmgtv.com",
                    "PPMGTV NewsLetter",
                    user.Email,
                    "Вижте най-новото от PPMGTV.com",
                    string.Format(EmailTemplates.SubscriptionLetterEmail,video.Title)
                    );
                context.WriteLine($"NewsLetter sent to {user.UserName}.");
            }
            context.WriteLine("Sending newsletters has finished successfully.");
        }

    }
}