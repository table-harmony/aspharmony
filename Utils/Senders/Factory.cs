using Microsoft.Extensions.Configuration;

namespace Utils.Senders {

    public enum SenderType {
        sms,
        whatsapp,
        email
    }

    public interface ISenderFactory {
        public ISender CreateSender(SenderType type);
    }

    public class SenderFactory(IConfiguration configuration) : ISenderFactory {
        public ISender CreateSender(SenderType type) {
            return type switch {
                SenderType.sms => new SMSSender(configuration),
                SenderType.whatsapp => new WhatsAppSender(configuration),
                SenderType.email => new EmailSender(configuration),
                _ => throw new InvalidOperationException($"Invalid sender: {type}"),
            };
        }
    }
}
