namespace Utils {
    public static class ConnectionStringBuilder {

        public static string GenerateConnectionString(string databaseName) {
            string path = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "Storage", "App_Data", databaseName);
            path = Path.GetFullPath(path);

            string connectionString =
                @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =" + path + "; Integrated Security = True";

            return connectionString;
        }
    }
}
