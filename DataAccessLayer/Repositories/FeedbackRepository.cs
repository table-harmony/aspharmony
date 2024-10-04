using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using System.Xml.Linq;

namespace DataAccessLayer.Repositories {
    public interface IFeedbackRepository {
        Task<DataSet> GetAllAsync();
        Task<DataSet> GetAsync(int id);
        Task CreateAsync(Feedback row);
        Task UpdateAsync(Feedback row);
        Task DeleteAsync(int id);
    }

    public class FeedbackRepository : IFeedbackRepository {
        private readonly ApplicationContext _context;
        private readonly string _connectionString;

        public FeedbackRepository(ApplicationContext context) {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        public async Task<DataSet> GetAllAsync() {
            string query =  @"SELECT f.Id, f.Title, f.Description, f.Label, f.UserId, u.UserName 
                                FROM Feedbacks f
                                LEFT JOIN AspNetUsers u ON f.UserId = u.Id";

            return await ExecuteQueryAsync(query);
        }

        public async Task<DataSet> GetAsync(int id) {
            string query = "GetFeedback";
            var parameters = new[] { new SqlParameter("@Id", id) };

            return await ExecuteQueryAsync(query, parameters, true);
        }

        public async Task CreateAsync(Feedback feedback) {
            string query = @"INSERT INTO Feedbacks (UserId, Title, Description, Label) 
                          VALUES (@UserId, @Title, @Description, @Label);
                          SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new[] {
                new SqlParameter("@UserId", feedback.UserId),
                new SqlParameter("@Title", feedback.Title),
                new SqlParameter("@Description", feedback.Description),
                new SqlParameter("@Label", feedback.Label)
            };

            var result = await ExecuteQueryAsync(query, parameters);
            if (result.Tables[0].Rows.Count > 0) {
                feedback.Id = int.Parse(result.Tables[0].Rows[0][0].ToString());
            }
        }

        public async Task UpdateAsync(Feedback feedback) {
            string query = @"UPDATE Feedbacks
                          SET UserId = @UserId, Title = @Title, Description = @Description, Label = @Label 
                          WHERE Id = @Id";

            var parameters = new[] {
                new SqlParameter("@Id", feedback.Id),
                new SqlParameter("@UserId", feedback.UserId),
                new SqlParameter("@Title", feedback.Title),
                new SqlParameter("@Description", feedback.Description),
                new SqlParameter("@Label", feedback.Label)
            };

            await ExecuteQueryAsync(query, parameters);
        }

        public async Task DeleteAsync(int id) {
            string query = "DeleteFeedback";
            var parameters = new[] { new SqlParameter("@Id", id) };

            await ExecuteQueryAsync(query, parameters, true);
        }

        private async Task<DataSet> ExecuteQueryAsync(string query, 
                                                        SqlParameter[] parameters = null,
                                                        bool isStoredProcedure = false) {
            using (var connection = new SqlConnection(_connectionString)) {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection)) {
                    if (isStoredProcedure)
                        command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null) {
                        command.Parameters.AddRange(parameters);
                    }

                    var dataSet = new DataSet();
                    using (var adapter = new SqlDataAdapter(command)) {
                        await Task.Run(() => adapter.Fill(dataSet));
                    }
                    return dataSet;
                }
            }
        }
    }
}