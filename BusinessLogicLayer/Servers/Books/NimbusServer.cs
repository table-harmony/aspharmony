using BusinessLogicLayer.Services;

using DbChapter = DataAccessLayer.Entities.BookChapter;
using DbMetadata = DataAccessLayer.Entities.BookMetadata;

namespace BusinessLogicLayer.Servers.Books {
    public class NimbusServer(IBookMetadataService metadataService,
                                IBookChapterService chapterService) : IBookServer {
        public async Task<Book?> GetBookAsync(int id) {
            var metadata = await metadataService.GetAsync(id);

            if (metadata == null)
                return null;

            var chapters = await chapterService.GetChaptersAsync(id);

            return new Book {
                Id = metadata.BookId,
                Title = metadata?.Title ?? "",
                Description = metadata?.Description ?? "",
                ImageUrl = metadata?.ImageUrl ?? "",
                Chapters = chapters.Select(chapter => new Chapter() {
                    Index = chapter.Index,
                    Title = chapter.Title,
                    Content = chapter.Content,
                }).ToList(),
            };
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            var metadatas = await metadataService.GetAllAsync();

            if (metadatas == null)
                return [];

            var books = await Task.WhenAll(
                metadatas.Select(async metadata => await GetBookAsync(metadata.BookId))
            );

            return [.. books];
        }

        public async Task CreateBookAsync(Book newBook) {
            await metadataService.CreateAsync(new DbMetadata {
                BookId = newBook.Id,
                Title = newBook.Title,
                Description = newBook.Description,
                ImageUrl = newBook.ImageUrl,
            });

            await Task.WhenAll(newBook.Chapters.Select(chapter =>
                chapterService.CreateAsync(new DbChapter {
                    BookId = newBook.Id,
                    Index = chapter.Index,
                    Title = chapter.Title,
                    Content = chapter.Content,
                })
            ));
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            await metadataService.DeleteAsync(updatedBook.Id);
            await metadataService.CreateAsync(new DbMetadata() {
                BookId = updatedBook.Id,
                Title = updatedBook.Title,
                Description = updatedBook.Description,
                ImageUrl = updatedBook.ImageUrl,
            });

            await chapterService.DeleteAsync(updatedBook.Id);
            foreach (var chapter in updatedBook.Chapters)
                await chapterService.CreateAsync(new DbChapter {
                    BookId = updatedBook.Id,
                    Index = chapter.Index,
                    Title = chapter.Title,
                    Content = chapter.Content,
                });
        }

        public async Task DeleteBookAsync(int id) {
            await Task.WhenAll(
                metadataService.DeleteAsync(id),
                chapterService.DeleteAsync(id)
            );
        }
    }
}