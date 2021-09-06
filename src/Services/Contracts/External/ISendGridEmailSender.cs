using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Services.Contracts.External
{
    public interface ISendGridEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string sender,
                                         string senderName,
                                         string reciever,
                                         string subject,
                                         string htmlMessage);
    }
}
