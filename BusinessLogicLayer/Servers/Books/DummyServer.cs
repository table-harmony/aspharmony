namespace BusinessLogicLayer.Servers.Books {
    public class DummyServer : IBookServer {

        public async Task<Book?> GetBookAsync(int id) {
            return await Task.FromResult(GenerateDummyBook(id));
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            List<Book> books = [];
            books.Add(GenerateDummyBook(0));

            return await Task.FromResult(books);
        }

        public async Task CreateBookAsync(Book newBook) {
            await Task.CompletedTask;
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            await Task.CompletedTask;
        }

        public async Task DeleteBookAsync(int id) {
            await Task.CompletedTask;
        }

        public static Book GenerateDummyBook(int id) {
            return new Book() {
                Id = id,
                Title = "Dummy title",
                Description = "Dummy Description",
                ImageUrl = "https://dummyimage.com/50x50/000/fff",
                Chapters = [
                    new Chapter() {
                        Index = 0,
                        Title = "Dummy title",
                        Content = "Dummy content"
                    }
                ]
            };
        }
    }
}
