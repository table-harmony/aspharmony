using DataAccessLayer.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Repositories.Nimbus {
    public interface INimbusFactory {
        (IBookMetadataRepository, IBookChapterRepository) Create(ServerType serverType);
    }

    public class NimbusFactory(IServiceProvider serviceProvider) : INimbusFactory {
        public (IBookMetadataRepository, IBookChapterRepository) Create(ServerType serverType) {
            return serverType switch {
                ServerType.Nimbus1 => (
                    serviceProvider.GetRequiredService<v1.BookMetadataRepository>(),
                    serviceProvider.GetRequiredService<v1.BookChapterRepository>()
                ),
                ServerType.Nimbus2 => (
                    serviceProvider.GetRequiredService<v2.BookMetadataRepository>(),
                    serviceProvider.GetRequiredService<v2.BookChapterRepository>()
                ),
                _ => throw new ArgumentException("Invalid server type for Nimbus repository", nameof(serverType))
            };
        }
    }
}