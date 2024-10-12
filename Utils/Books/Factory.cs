using LocalBooksServiceReference;
using ForeignBooksServiceReference;
using Microsoft.Extensions.Configuration;

namespace Utils.Books
{
    public static class BooksServiceFactory {
        public static IBooksWebService CreateService(IConfiguration configuration) {
            var environment = configuration["Environment"];

            return environment switch {
                "Production" => new ForeignBooksService(new BooksServicePortTypeClient(BooksServicePortTypeClient.EndpointConfiguration.BooksServicePort)),
                "Development" => new LocalBooksService(new BooksServiceSoapClient(BooksServiceSoapClient.EndpointConfiguration.BooksServiceSoap)),
                _ => throw new Exception("Invalid environment"),
            };
        }
    }
}