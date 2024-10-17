using LocalBooksServiceReference;
using ForeignBooksServiceReference;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogicLayer.Services.Nimbus;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Configuration;
using Utils.Exceptions;

namespace BusinessLogicLayer.Servers.Books {
    public static class BooksServerFactory {
        public static Func<ServerType, IBookServer> CreateServer(IServiceProvider serviceProvider) {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var serverSettings = configuration.GetSection("ServerSettings");
            var enabledServers = new HashSet<ServerType>(
                serverSettings.GetSection("EnabledServers").Get<List<ServerType>>() ?? []);

            return (serverType) => {
                if (!enabledServers.Contains(serverType)) {
                    throw new PublicException($"The server '{serverType}' is not enabled");
                }

                return serverType switch {
                    ServerType.Echo => new EchoServer(),
                    ServerType.Atlas => new AtlasServer(),
                    ServerType.Solace => new SolaceServer(),
                    ServerType.Aether => new AetherServer(new BooksServicePortTypeClient(BooksServicePortTypeClient.EndpointConfiguration.BooksServicePort)),
                    ServerType.Orion => new OrionServer(new BooksServiceSoapClient(BooksServiceSoapClient.EndpointConfiguration.BooksServiceSoap)),
                    ServerType.Nimbus => new NimbusServer(serviceProvider.GetRequiredService<IBookMetadataService>(),
                                                    serviceProvider.GetRequiredService<IBookChapterService>()),
                    _ => throw new InvalidOperationException($"Invalid server type: {serverType}"),
                };
            };
        }
    }
}