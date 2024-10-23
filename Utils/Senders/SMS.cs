using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Utils.Senders {
    public class SMSSender : ISender {
        private readonly string from;

        public SMSSender(IConfiguration configuration) {
            from = configuration["Twilio:Numbers:SMS"]!;

            string accountSid = configuration["Twilio:AccountSid"]!;
            string authToken = configuration["Twilio:AuthToken"]!;
            TwilioClient.Init(accountSid, authToken);
        }

        public void Send(string body, string to) {
            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(to)) {
                From = new PhoneNumber(from),
                Body = body
            };

            MessageResource.Create(messageOptions);
        }
    }
}
