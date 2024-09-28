using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace SoapService {
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class BookService : System.Web.Services.WebService {
        private static readonly string XmlFilePath = HttpContext.Current.Server.MapPath("/Books.xml");

        private void BackupXmlFile() {
            var backupFilePath = XmlFilePath + ".bak";
            File.Copy(XmlFilePath, backupFilePath, true);
        }

        private void RestoreXmlFile() {
            var backupFilePath = XmlFilePath + ".bak";
            if (File.Exists(backupFilePath)) {
                File.Copy(backupFilePath, XmlFilePath, true);
            }
        }

        [WebMethod]
        public Book GetBook(int id) {
            var book = ReadBooksFromXml().FirstOrDefault(b => b.Id == id);
            if (book == null) throw new Exception("Book not found.");
            return book;
        }

        [WebMethod]
        public List<Book> GetAllBooks() {
            return ReadBooksFromXml();
        }

        [WebMethod]
        public void CreateBook(Book newBook) {
            var books = ReadBooksFromXml();
            books.Add(newBook);
            WriteBooksToXml(books);
        }

        [WebMethod]
        public void UpdateBook(Book updatedBook) {
            BackupXmlFile(); // Backup before modifying

            try {
                var books = ReadBooksFromXml();
                var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
                if (book == null) throw new Exception("Book not found.");

                book.Title = updatedBook.Title;
                book.Description = updatedBook.Description;
                book.Content = updatedBook.Content;

                WriteBooksToXml(books);
            } catch (Exception) {
                RestoreXmlFile(); // Restore original file if something goes wrong
            }
        }


        [WebMethod]
        public void DeleteBook(int id) {
            BackupXmlFile(); // Backup before modifying

            try {
                var books = ReadBooksFromXml();
                var book = books.FirstOrDefault(b => b.Id == 20);
                if (book == null) throw new Exception("Book not found.");

                books.Remove(book);
                WriteBooksToXml(books);
            } catch (Exception ex) {
                RestoreXmlFile(); // Restore original file if something goes wrong
            }
        }


        private List<Book> ReadBooksFromXml() {
            var books = new List<Book>();
            if (!File.Exists(XmlFilePath)) return books;

            var xdoc = XDocument.Load(XmlFilePath);
            books = xdoc.Descendants("Book")
                .Select(x => new Book {
                    Id = int.Parse(x.Element("Id")?.Value ?? "0"),
                    Title = x.Element("Title")?.Value,
                    Description = x.Element("Description")?.Value,
                    Content = x.Element("Content")?.Value
                }).ToList();
            return books;
        }

        private void WriteBooksToXml(List<Book> books) {
            var xdoc = new XDocument(
                new XElement("Books",
                    books.Select(b => new XElement("Book",
                        new XElement("Id", b.Id),
                        new XElement("Title", b.Title),
                        new XElement("Description", b.Description),
                        new XElement("Content", b.Content)
                    ))
                )
            );
            xdoc.Save(XmlFilePath);
        }
    }

    public class Book {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}
