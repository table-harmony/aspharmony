using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccessLayer.Data {
    public class AdoContext(string connectionString) {

        public DataSet ExecuteQuery(string query, SqlParameter[]? parameters = null, bool isStoredProcedure = false) {
            using SqlConnection connection = new(connectionString);
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
            using SqlConnection connection = new(connectionString);
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

    }

}
