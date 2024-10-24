using LocalBooksServiceReference;
using ForeignBooksServiceReference;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogicLayer.Services.Nimbus;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Utils.Exceptions;
using Utils;

using SteganMetadata = BusinessLogicLayer.Services.Stegan.IBookMetadataService;
using DataAccessLayer.Repositories.Nimbus;

namespace BusinessLogicLayer.Servers.Books {
    public static class BooksServerFactory {
        public static Func<ServerType, IBookServer> CreateServer(IServiceProvider serviceProvider) {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var serverSettings = configuration.GetSection("ServerSettings");
            var disabledServers = new HashSet<ServerType>(
                serverSettings.GetSection("DisabledServers").Get<List<ServerType>>() ?? []);

            return (serverType) => {
                if (disabledServers.Contains(serverType)) {
                    throw new PublicException($"The server '{serverType}' is disabled");
                }

                return serverType switch {
                    ServerType.Aether => new AetherServer(new BooksServicePortTypeClient(BooksServicePortTypeClient.EndpointConfiguration.BooksServicePort)),
                    ServerType.Atlas => new ApiServer("https://localhost:7137", new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }),
                    ServerType.Dummy => new DummyServer(),
                    ServerType.Echo => new EchoServer(),
                    ServerType.Harmony => new ApiServer($"http://{configuration["MINECRAFT_SERVICE_IP_ADDRESS"]}:8000"),
                    ServerType.Nimbus1 => CreateNimbusServer(serviceProvider, ServerType.Nimbus1),
                    ServerType.Nimbus2 => CreateNimbusServer(serviceProvider, ServerType.Nimbus2),
                    ServerType.Orion => new OrionServer(new BooksServiceSoapClient(BooksServiceSoapClient.EndpointConfiguration.BooksServiceSoap)),
                    ServerType.Solace => new SolaceServer(),
                    ServerType.Stegan1 => new Stegan1Server(),
                    ServerType.Stegan2 => new Stegan2Server(serviceProvider.GetRequiredService<SteganMetadata>(),
                                                               serviceProvider.GetRequiredService<IFileUploader>()),
                    _ => throw new InvalidOperationException($"Invalid server type: {serverType}"),
                };
            };
        }

        private static NimbusServer CreateNimbusServer(IServiceProvider serviceProvider, ServerType serverType) {
            var nimbusFactory = serviceProvider.GetRequiredService<INimbusFactory>();

            (var metadataRepository, var chaptersRepository) = nimbusFactory.Create(serverType);

            var metadataService = new BookMetadataService(metadataRepository);
            var chaptersService = new BookChapterService(chaptersRepository);

            return new NimbusServer(metadataService, chaptersService);
        }
    }
}