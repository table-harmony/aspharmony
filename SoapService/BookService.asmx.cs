using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace SoapService {
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class BookService : System.Web.Services.WebService {
        private static readonly string XmlFilePath = HttpContext.Current.Server.MapPath("/Books.xml");

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
            var books = ReadBooksFromXml();
            var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (book == null) throw new Exception("Book not found.");

            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.Content = updatedBook.Content;
            WriteBooksToXml(books);
        }

        [WebMethod]
        public void DeleteBook(int id) {
            var books = ReadBooksFromXml();
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) throw new Exception("Book not found.");
            books.Remove(book);
            WriteBooksToXml(books);
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
