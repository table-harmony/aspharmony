using LocalBooksServiceReference;
using ForeignBooksServiceReference;
using Microsoft.Extensions.Configuration;
using BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Servers.Books {
    public static class BooksServerFactory {
        public static IBookServer CreateService(IConfiguration configuration, IServiceProvider serviceProvider) {
            string? booksServer = configuration["BOOKS_SERVER"];

            return booksServer switch {
                "Atlas" => new AtlasServer(),
                "Echo" => new EchoServer(),
                "Solace" => new SolaceServer(),
                "Aether" => new AetherServer(new BooksServicePortTypeClient(BooksServicePortTypeClient.EndpointConfiguration.BooksServicePort)),
                "Orion" => new OrionServer(new BooksServiceSoapClient(BooksServiceSoapClient.EndpointConfiguration.BooksServiceSoap)),
                "Nimbus" => new NimbusServer(serviceProvider.GetRequiredService<IBookMetadataService>(),
                                                serviceProvider.GetRequiredService<IBookChapterService>()),
                _ => throw new Exception("Invalid service"),
            };
        }
    }
}