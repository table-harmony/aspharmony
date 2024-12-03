using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Utils.Exceptions;
using Utils.Senders.Configuration;

namespace Utils.Senders {
    public class WhatsAppSender : ISender {
        private readonly TwilioConfig _config;

        public WhatsAppSender(IConfiguration configuration) {
            _config = new TwilioConfig {
                AccountSid = configuration["Twilio:AccountSid"]!,
                AuthToken = configuration["Twilio:AuthToken"]!,
                SmsNumber = configuration["Twilio:Numbers:SMS"]!,
                WhatsAppNumber = configuration["Twilio:Numbers:WhatsApp"]!
            };

            TwilioClient.Init(_config.AccountSid, _config.AuthToken);
        }

        public void Send(string body, string to) {
            try {
                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber($"whatsapp:{to}")) {
                    From = new PhoneNumber($"whatsapp:{_config.WhatsAppNumber}"),
                    Body = body
                };

                MessageResource.CreateAsync(messageOptions);
            } catch {
                throw new SenderException();
            }
        }
    }
}