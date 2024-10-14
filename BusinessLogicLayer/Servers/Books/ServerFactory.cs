using LocalBooksServiceReference;
using ForeignBooksServiceReference;
using BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Servers.Books {

    public static class BooksServerFactory {
        public static Func<int, Task<IBookServer>> CreateServer(IServiceProvider serviceProvider) {
            var serverService = serviceProvider.GetRequiredService<IServerService>();

            return async (serverId) => {
                var server = await serverService.GetAsync(serverId);

                return server?.Name switch {
                    "Echo" => new EchoServer(),
                    "Atlas" => new AtlasServer(),
                    "Solace" => new SolaceServer(),
                    "Aether" => new AetherServer(new BooksServicePortTypeClient(BooksServicePortTypeClient.EndpointConfiguration.BooksServicePort)),
                    "Orion" => new OrionServer(new BooksServiceSoapClient(BooksServiceSoapClient.EndpointConfiguration.BooksServiceSoap)),
                    "Nimbus" => new NimbusServer(serviceProvider.GetRequiredService<IBookMetadataService>(),
                                                    serviceProvider.GetRequiredService<IBookChapterService>()),
                    _ => throw new Exception("Invalid server"),
                };
            };
        }
    }
}

