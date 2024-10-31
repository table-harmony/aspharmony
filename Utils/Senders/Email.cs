using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Utils.Exceptions;
using Utils.Senders.Configuration;

namespace Utils.Senders {
    public class EmailSender : ISender {
        private readonly EmailConfig _config;

        public EmailSender(IConfiguration configuration) {
            _config = new EmailConfig {
                From = configuration["Email:From"]!,
                Username = configuration["Email:Username"]!,
                Password = configuration["Email:Password"]!,
                Host = configuration["Email:Host"]!,
                Port = int.Parse(configuration["Email:Port"]!)
            };
        }

        public void Send(string message, string to) {
            try {
                MailMessage mail = new() {
                    From = new MailAddress(_config.From, "AspHarmony"),
                    Subject = "Message from AspHarmony",
                    Body = message,
                    IsBodyHtml = false
                };

                mail.To.Add(to);

                using SmtpClient client = new() {
                    Host = _config.Host,
                    Port = _config.Port,
                    Credentials = new NetworkCredential(_config.Username, _config.Password),
                    EnableSsl = true
                };

                client.Send(mail);
            } catch { 
                throw;
                throw new SenderException();
            }
        }
    }
}