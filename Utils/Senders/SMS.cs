using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Utils.Exceptions;
using Utils.Senders.Configuration;

namespace Utils.Senders {
    public class SMSSender : ISender {
        private readonly TwilioConfig _config;

        public SMSSender(IConfiguration configuration) {
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
                CreateMessageOptions options = new(new PhoneNumber(to)) {
                    From = new PhoneNumber(_config.SmsNumber),
                    Body = body
                };

                MessageResource.CreateAsync(options);
            } catch {
                throw new SenderException();
            }
        }
    }
}