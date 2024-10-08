using System.Data;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

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
        private readonly string _xmlFilePath;

        public FeedbackRepository(ApplicationContext context, IConfiguration configuration) {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;

            _xmlFilePath = configuration["XmlPaths:Feedbacks"]!;
        }

        public async Task<DataSet> GetAllAsync() {
            string query =  @"SELECT f.Id, f.Title, f.Description, f.Label, f.UserId, u.UserName 
                                FROM Feedbacks f
                                LEFT JOIN AspNetUsers u ON f.UserId = u.Id";

            DataSet feedbacks = ExecuteQuery(query);
            await BackupToXmlAsync(feedbacks);

            return feedbacks;
        }

        public async Task<DataSet> GetAsync(int id) {
            string query = "GetFeedback";
            var parameters = new[] { new SqlParameter("@Id", id) };

            return ExecuteQuery(query, parameters, true);
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

            var result = ExecuteQuery(query, parameters);
            if (result.Tables[0].Rows.Count > 0) {
                feedback.Id = int.Parse(result.Tables[0].Rows[0][0].ToString()!);
            }

            await BackupToXmlAsync(feedback);
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

            ExecuteQuery(query, parameters);
            await BackupToXmlAsync(feedback);
        }

        public async Task DeleteAsync(int id) {
            string query = "DeleteFeedback";
            var parameters = new[] { new SqlParameter("@Id", id) };

            ExecuteQuery(query, parameters, true);

            await RemoveFromXmlAsync(id);
        }


        private DataSet ExecuteQuery(string query, SqlParameter[]? parameters = null, bool isStoredProcedure = false) {
            using SqlConnection connection = new(_connectionString);
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

        private async Task BackupToXmlAsync(Feedback feedback) {
            var xdoc = await Task.Run(() => {
                if (File.Exists(_xmlFilePath)) {
                    return XDocument.Load(_xmlFilePath);
                }
                return null;
            });

            if (xdoc == null)
                return;

            var element = xdoc.Root.Elements("Feedback")
                    .FirstOrDefault(e => e.Attribute("Id")?.Value == feedback.Id.ToString());

            if (element != null)
                return;

            xdoc.Root.Add(new XElement("Feedback",
                new XAttribute("Id", feedback.Id),
                new XElement("UserId", feedback.UserId),
                new XElement("Title", feedback.Title),
                new XElement("Description", feedback.Description),
                new XElement("Label", feedback.Label)
            ));

            await Task.Run(() => xdoc.Save(_xmlFilePath));
        }

        private async Task RemoveFromXmlAsync(int id) {
            var xdoc = await Task.Run(() => {
                if (File.Exists(_xmlFilePath)) {
                    return XDocument.Load(_xmlFilePath);
                }
                return null;
            });

            if (xdoc == null)
                return;

            var feedback = xdoc.Root.Elements("Feedback")
                .FirstOrDefault(e => e.Attribute("Id")?.Value == id.ToString());

            if (feedback == null)
                return;

            feedback.Remove();
            await Task.Run(() => xdoc.Save(_xmlFilePath));
        }

        private async Task BackupToXmlAsync(DataSet feedbacks) {
            var xdoc = await Task.Run(() => {
                if (File.Exists(_xmlFilePath)) {
                    return XDocument.Load(_xmlFilePath);
                }
                return null;
            });

            if (xdoc == null)
                return;

            foreach (DataRow row in feedbacks.Tables[0].Rows) {
                var feedback = xdoc.Root.Elements("Feedback")
                    .FirstOrDefault(e => e.Attribute("Id")?.Value == row["Id"].ToString());

                if (feedback != null)
                    continue;

                xdoc.Root.Add(new XElement("Feedback",
                    new XAttribute("Id", row["Id"]),
                    new XElement("UserId", row["UserId"].ToString()),
                    new XElement("Title", row["Title"].ToString()),
                    new XElement("Description", row["Description"].ToString()),
                    new XElement("Label", row["Label"])
                ));
            }

            await Task.Run(() => xdoc.Save(_xmlFilePath));
        }
    }
}