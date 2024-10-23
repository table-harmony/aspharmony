using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Utils.Senders {
    public class EmailSender(IConfiguration configuration) : ISender {
        public void Send(string message, string to) {
            var username = configuration["Email:SmtpUsername"];
            var password = configuration["Email:SmtpPassword"];
            var smtpHost = configuration["Email:SmtpHost"];
            var smtpPort = int.Parse(configuration["Email:SmtpPort"]!);

            MailMessage mail = new() {
                From = new MailAddress(username, "AspHarmony"),
                Subject = "AspHarmony",
                Body = message,
                IsBodyHtml = false
            };

            mail.To.Add(to);

            using SmtpClient client = new(smtpHost) {
                Port = smtpPort,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            client.Send(mail);
        }
    }
}
