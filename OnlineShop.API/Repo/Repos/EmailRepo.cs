using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using OnlineShop.API.Repo.Interfaces;

namespace OnlineShop.API.Repo.Repos
{
    public class EmailRepo : IEmail
    {
        public void SendMail(string body, string receiver, string subject)
        {
            IConfiguration configuration;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("624rsr@gmail.com"));
            email.To.Add(MailboxAddress.Parse(receiver));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            //mksh splm yvqe pmqg
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("624rsr@gmail.com", "knmt rtyq uzvy ypvf");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
