namespace BusinessLogicLayer.Servers.Books.Documents {
    public interface IDocumentStorage {
        void Save(List<Book> books);
        List<Book> Load();
    }
}
