using Utils;

namespace BusinessLogicLayer.Servers.Books.Documents {
    public enum DocumentType {
        Word,
        Excel,
        PowerPoint,
    }

    public class DocumentFactory {
        private static readonly Dictionary<DocumentType, string> DocumentSuffixes = new() {
            { DocumentType.Word, "docx" },
            { DocumentType.Excel, "xlsx" },
            { DocumentType.PowerPoint, "pptx" }
        };

        public static IDocumentStorage CreateDocumentStorage(DocumentType documentType) {
            string suffix = GetSuffix(documentType);
            string filePath = PathManager.GetPath(FolderType.Books, $"Solace.{suffix}");

            return documentType switch {
                DocumentType.Word => new WordDocumentStorage(filePath),
                DocumentType.Excel => new ExcelDocumentStorage(filePath),
                DocumentType.PowerPoint => new PowerPointStorage(filePath),
                _ => throw new ArgumentException("Unsupported document format.")
            };
        }

        public static string GetSuffix(DocumentType documentType) {
            if (!DocumentSuffixes.TryGetValue(documentType, out var suffix))
                throw new ArgumentException("Unsupported document format.");

            return suffix;
        }
    }
}
