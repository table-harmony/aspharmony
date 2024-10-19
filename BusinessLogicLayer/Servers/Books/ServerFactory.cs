using LocalBooksServiceReference;
using ForeignBooksServiceReference;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogicLayer.Services.Nimbus;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
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
                    ServerType.Aether => new AetherServer(new BooksServicePortTypeClient(BooksServicePortTypeClient.EndpointConfiguration.BooksServicePort)),
                    ServerType.Atlas => new ApiServer("https://localhost:7137", new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }),
                    ServerType.Dummy => new DummyServer(),
                    ServerType.Echo => new EchoServer(),
                    ServerType.Harmony => new ApiServer("http://localhost:8000"),
                    ServerType.Nimbus => new NimbusServer(serviceProvider.GetRequiredService<IBookMetadataService>(),
                                                    serviceProvider.GetRequiredService<IBookChapterService>()),
                    ServerType.Orion => new OrionServer(new BooksServiceSoapClient(BooksServiceSoapClient.EndpointConfiguration.BooksServiceSoap)),
                    ServerType.Solace => new SolaceServer(),
                    _ => throw new InvalidOperationException($"Invalid server type: {serverType}"),
                };
            };
        }
    }
}