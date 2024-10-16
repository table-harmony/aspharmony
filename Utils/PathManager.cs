namespace Utils {

    public enum FolderType {
        Books,
        Feedbacks,
        Databases
    }

    public static class PathManager {
        private static readonly Dictionary<FolderType, string> folderPaths = GenerateFolderPaths();

        private static Dictionary<FolderType, string> GenerateFolderPaths() {
            string storageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "Storage", "App_Data");

            return new Dictionary<FolderType, string> {
                { FolderType.Books, Path.Combine(storageDirectory, "Books") },
                { FolderType.Feedbacks, Path.Combine(storageDirectory, "Feedbacks") },
                { FolderType.Databases, storageDirectory }
            };
        }

        public static string GetFilePath(FolderType folder, string fileName) {
            string folderPath = folderPaths.TryGetValue(folder, out var path) ? path
                : throw new ArgumentException($"Folder '{folder}' is not recognized.");

            string fullPath = Path.Combine(folderPath, fileName);

            if (!File.Exists(fullPath)) {
                throw new FileNotFoundException($"File '{fileName}' not found in the specified storage.");
            }

            return fullPath;
        }

        public static string GenerateConnectionString(string fileName) {
            string path = Path.GetFullPath(GetFilePath(FolderType.Databases, fileName));
            return $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={path};Integrated Security=True";
        }
    }
}