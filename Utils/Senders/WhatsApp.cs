using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Utils.Senders {
    public class WhatsAppSender : ISender {
        private readonly string from;

        public WhatsAppSender(IConfiguration configuration) {
            string accountSid = configuration["Twilio:AccountSid"]!;
            string authToken = configuration["Twilio:AuthToken"]!;
            from = configuration["Twilio:Numbers:WhatsApp"]!;

            TwilioClient.Init(accountSid, authToken);
        }

        public void Send(string body, string to) {
            var messageOptions = new CreateMessageOptions(
                new PhoneNumber($"whatsapp:{to}")) {
                From = new PhoneNumber($"whatsapp:{from}"),
                Body = body
            };

            MessageResource.Create(messageOptions);
        }
    }
}
