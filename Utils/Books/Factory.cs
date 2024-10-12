using LocalBooksServiceReference;
using ForeignBooksServiceReference;
using Microsoft.Extensions.Configuration;

namespace Utils.Books {
    public static class BooksServiceFactory {
        public static IBooksWebService CreateService(IConfiguration configuration) {
            var bookService = configuration["BOOKS_SERVICE_TYPE"];

            return bookService switch {
                "Api" => new ApiBooksService(),
                "ForeignWebService" => new ForeignBooksService(new BooksServicePortTypeClient(BooksServicePortTypeClient.EndpointConfiguration.BooksServicePort)),
                "LocalWebService" => new LocalBooksService(new BooksServiceSoapClient(BooksServiceSoapClient.EndpointConfiguration.BooksServiceSoap)),
                _ => throw new Exception("Invalid service"),
            };
        }
    }
}