using SendGrid;
using SendGrid.Helpers.Mail;
using Services.Contracts.External;
using System.Threading.Tasks;

namespace Services.External
{
    public class SendGridEmailSender : ISendGridEmailSender
    {
        private readonly SendGridClient client;
        private readonly string sender;
        private readonly string senderName;

        public SendGridEmailSender(string APIKey, string sender, string senderName)
        {
            this.client = new SendGridClient(APIKey);
            this.sender = sender;
            this.senderName = senderName;
        }

        /// <summary>
        /// Sends email from a selected sender to a selected receiver with subject and message
        /// </summary>
        /// <param name="sender">Email of the sender</param>
        /// <param name="senderName">Name of the sender</param>
        /// <param name="reciever">Email of the reciever</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="htmlMessage">Message to send</param>
        public async Task SendEmailAsync(string sender,
                                         string senderName,
                                         string reciever,
                                         string subject,
                                         string htmlMessage)
        {
            var from = new EmailAddress(sender, senderName);
            var to = new EmailAddress(reciever);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlMessage);
            await client.SendEmailAsync(msg);
        }

        /// <summary>
        /// Sends email from a default sender to a selected receiver with subject and message
        /// </summary>
        /// <param name="reciever">Email of the reciever</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="htmlMessage">Message to send</param>
        public async Task SendEmailAsync(string reciever,
                                         string subject,
                                         string htmlMessage)
        {
            var from = new EmailAddress(sender, senderName);
            var to = new EmailAddress(reciever);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlMessage);
            var response = await client.SendEmailAsync(msg);
            System.Console.WriteLine(response.StatusCode);
        }

    }
}
