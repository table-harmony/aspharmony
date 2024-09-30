using System;
using System.Collections.Generic;
using System.Data;
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

        private void BackupXmlFile() {
            var backupFilePath = XmlFilePath + ".bak";
            var xdoc = XDocument.Load(XmlFilePath);
            xdoc.Save(backupFilePath);
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
            if (book == null) return null;
            return book;
        }

        [WebMethod]
        public List<Book> GetAllBooks() {
            return ReadBooksFromXml();
        }

        [WebMethod]
        public void CreateBook(Book newBook) {
            BackupXmlFile();

            try {
                var books = ReadBooksFromXml();
                books.Add(newBook);

                WriteBooksToXml(books);
            } catch {
                RestoreXmlFile();
            }
        }

        [WebMethod]
        public void UpdateBook(Book updatedBook) {
            BackupXmlFile();

            try {
                var books = ReadBooksFromXml();
                var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
                if (book == null) return;

                book.Title = updatedBook.Title;
                book.Description = updatedBook.Description;
                book.Chapters = updatedBook.Chapters;

                WriteBooksToXml(books);
            } catch {
                RestoreXmlFile();
            }
        }


        [WebMethod]
        public void DeleteBook(int id) {
            BackupXmlFile();

            try {
                var books = ReadBooksFromXml();
                var book = books.FirstOrDefault(b => b.Id == id);
                if (book == null) return;

                books.Remove(book);
                WriteBooksToXml(books);
            } catch {
                RestoreXmlFile();
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
                    Chapters = x.Element("Chapters")?.Elements("Chapter")
                        .Select(c => new Chapter {
                            Index = int.Parse(c.Attribute("index")?.Value ?? "0"),
                            Title = c.Element("Title")?.Value,
                            Content = c.Element("Content")?.Value
                        }).ToList() ?? new List<Chapter>()
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
                        new XElement("Chapters",
                            b.Chapters.Select(c => new XElement("Chapter",
                                new XAttribute("index", c.Index),
                                new XElement("Title", c.Title),
                                new XElement("Content", c.Content)
                            ))
                        )
                    ))
                )
            );
            xdoc.Save(XmlFilePath);
        }

        private void WriteBooksToXml(DataSet books) {
            List<Book> list = new List<Book>();

            foreach (DataRow row in books.Tables[0].Rows) {
                Book book = new Book {
                    Id = int.Parse(row["id"].ToString()),
                    Title = row["title"].ToString(),
                    Description = row["description"].ToString(),
                    Chapters = row["chapters"].ToString() == null ? null :
                        row["chapters"].ToString()
                            .Split(',')
                            .Select(c => new Chapter { 
                                Index = int.Parse(c.Split(':')[0]), 
                                Title = c.Split(':')[1], 
                                Content = c.Split(':')[2] 
                            }).ToList() 
                };

                list.Add(book);
            }

            WriteBooksToXml(list);
        }
    }

    public class Book {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();
    }

    public class Chapter {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
