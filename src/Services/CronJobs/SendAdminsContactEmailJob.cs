using Common.Constants;
using Data.Contracts.Repositories;
using Data.Models;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.Identity;
using Services.Contracts.CronJobs;
using Services.Contracts.External;
using System.Linq;
using System.Threading.Tasks;

namespace Services.CronJobs
{
    public class SendAdminsContactEmailJob
    {
        private readonly IDeletableEntityRepository<ApplicationUser> repository;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ISendGridEmailSender emailSender;

        public SendAdminsContactEmailJob(
            IDeletableEntityRepository<ApplicationUser> repository,
            RoleManager<ApplicationRole> roleManager,
            ISendGridEmailSender emailSender
            )
        {
            this.repository = repository;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }

        [AutomaticRetry(Attempts = 2)]
        public async Task Work(ContactLetter letter, PerformContext context)
        {
            var roles = roleManager.Roles.Where(x => x.Name == ApplicationRolesNames.AdminRole || x.Name == ApplicationRolesNames.EditorRole).Select(x => x.Id).ToList();
            var admins = repository.All().Where(x => x.Roles.Select(x => x.RoleId).Any(y => roles.Contains(y)));
            foreach (var user in admins)
            {
                await emailSender.SendEmailAsync(
                    user.Email,
                    "New contact letter has been sent!",
                    string.Format(EmailTemplates.ContactLetterEmail, letter.About, letter.Description, letter.SenderName, letter.SenderEmail, letter.OtherContactInfo)
                    );
                context.WriteLine($"Contact letter from {letter.SenderName} forwarded to {user.UserName}");
            }

            context.WriteLine("Forwarding contact letters finished successfully.");
        }
    }
}