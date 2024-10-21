using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccessLayer.Data {
    public class AdoContext(string connectionString) {

        public SqlConnection GetConnection() {
            return new SqlConnection(connectionString);
        }

        public DataSet ExecuteQuery(string query, SqlParameter[]? parameters = null, bool isStoredProcedure = false) {
            using SqlConnection connection = GetConnection();
            connection.Open();

            using SqlCommand command = new(query, connection);

            if (isStoredProcedure)
                command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using SqlDataAdapter adapter = new(command);
            DataSet dataSet = new();
            adapter.Fill(dataSet);

            return dataSet;
        }

        public async Task<DataSet> ExecuteQueryAsync(string query, SqlParameter[]? parameters = null, bool isStoredProcedure = false) {
            using SqlConnection connection = GetConnection();
            await connection.OpenAsync();

            using SqlCommand command = new(query, connection);

            if (isStoredProcedure)
                command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using SqlDataAdapter adapter = new(command);
            DataSet dataSet = new();
            adapter.Fill(dataSet);

            return dataSet;
        }

        public DataSet ExecuteFunction(string functionName, SqlParameter[]? parameters = null) {
            string parmetersString = "";
            if (parameters != null && parameters.Length != 0) {
                parmetersString = string.Join(", ", parameters.Select(p => p.Value == null ? "NULL" : p.ParameterName));
            }

            string sql = $"SELECT * FROM dbo.{functionName}({parmetersString})";

            using SqlConnection connection = new(connectionString);
            connection.Open();

            using SqlCommand command = new(sql, connection);

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using SqlDataAdapter adapter = new(command);
            DataSet dataSet = new();
            adapter.Fill(dataSet);

            return dataSet;
        }
    }
}
