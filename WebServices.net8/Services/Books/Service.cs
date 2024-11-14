using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml;
using Utils;

namespace WebServices.net8.Services.Books {
    public class BooksService : IBooksService {
        private static readonly string filePath = PathManager.GetPath(FolderType.Books, "Orion.xml");
        private static readonly string schemaPath = PathManager.GetPath(FolderType.Books, "Orion.xsd");
        private static readonly string backupFilePath = PathManager.GetPath(FolderType.Books, "Orion.xml.bak");

        public Book? GetBook(int id) {
            return ReadBooks().FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Book> GetAllBooks() {
            return ReadBooks();
        }

        public void CreateBook(Book newBook) {
            var books = ReadBooks();
            books.ToList().Add(newBook);
            SaveBooks(books);
        }

        public void UpdateBook(Book updatedBook) {
            var books = ReadBooks();

            var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (book == null) return;

            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.ImageUrl = updatedBook.ImageUrl;
            book.Chapters = updatedBook.Chapters;

            SaveBooks(books);
        }

        public void DeleteBook(int id) {
            var books = ReadBooks();
            
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return;

            books.ToList().Remove(book);
        }

        private static IEnumerable<Book> ReadBooks() {
            if (!File.Exists(filePath)) return [];

            try {
                var schemas = new XmlSchemaSet();
                using var schemaReader = XmlReader.Create(schemaPath);
                schemas.Add("", schemaReader);
                
                var settings = new XmlReaderSettings {
                    ValidationType = ValidationType.Schema,
                    Schemas = schemas,
                };

                using var reader = XmlReader.Create(filePath, settings);
                var serializer = new XmlSerializer(typeof(IEnumerable<Book>));
                return serializer.Deserialize(reader) as IEnumerable<Book> ?? [];
            } catch {
                return [];
            }
        }

        private static void SaveBooks(IEnumerable<Book> books) {
            BackupXmlFile();

            try {
                using var writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true });
                var serializer = new XmlSerializer(typeof(IEnumerable<Book>));
                serializer.Serialize(writer, books);
            } catch {
                RestoreXmlFile();
            }
        }

        private static void BackupXmlFile() {
            if (File.Exists(filePath)) {
                File.Copy(filePath, backupFilePath, overwrite: true);
            }
        }

        private static void RestoreXmlFile() {
            if (File.Exists(backupFilePath)) {
                File.Copy(backupFilePath, filePath, overwrite: true);
            }
        }
    }
}