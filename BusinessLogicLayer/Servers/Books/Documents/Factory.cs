namespace BusinessLogicLayer.Servers.Books.Documents {
    public class DocumentFactory {
        public static IDocumentStorage CreateDocumentStorage(string format) {
            string filePath = GetFilePath(format);

            return format.ToLower() switch {
                "word" => new WordDocumentStorage(filePath),
                "excel" => new ExcelDocumentStorage(filePath),
                "ppt" => new PowerPointDocumentSaver(filePath),
                _ => throw new ArgumentException("Unsupported document format.")
            };
        }

        public static string GetFilePath(string format) {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "Storage", "App_Data", "Books", "Solace");

            return format.ToLower() switch {
                "word" => filePath + ".docx",
                "excel" => filePath + ".xlsx",
                "ppt" => filePath + ".pptx",
                _ => throw new ArgumentException("Unsupported document format.")
            };
        }
    }

}
