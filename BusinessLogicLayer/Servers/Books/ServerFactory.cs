using LocalBooksServiceReference;
using ForeignBooksServiceReference;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogicLayer.Services.Nimbus;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Servers.Books {
    public static class BooksServerFactory {
        public static Func<ServerType, IBookServer> CreateServer(IServiceProvider serviceProvider) {
            return (serverType) => {
                return serverType switch {
                    ServerType.Echo => new EchoServer(),
                    ServerType.Atlas => new AtlasServer(),
                    ServerType.Solace => new SolaceServer(),
                    ServerType.Aether => new AetherServer(new BooksServicePortTypeClient(BooksServicePortTypeClient.EndpointConfiguration.BooksServicePort)),
                    ServerType.Orion => new OrionServer(new BooksServiceSoapClient(BooksServiceSoapClient.EndpointConfiguration.BooksServiceSoap)),
                    ServerType.Nimbus => new NimbusServer(serviceProvider.GetRequiredService<IBookMetadataService>(),
                                                    serviceProvider.GetRequiredService<IBookChapterService>()),
                    _ => throw new Exception("Invalid server"),
                };
            };
        }
    }
}

