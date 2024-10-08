using System.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services {
    public interface IFeedbackService {
        Task<DataSet> GetAllAsync();
        Task<Feedback?> GetAsync(int id);
        Task CreateAsync(Feedback feedback);
        Task UpdateAsync(Feedback feedback);
        Task DeleteAsync(int id);
    }

    public class FeedbackService(IFeedbackRepository repository) : IFeedbackService {
        public async Task<DataSet> GetAllAsync() {
            return await repository.GetAllAsync();
        }

        public async Task<Feedback?> GetAsync(int id) {
            var dataSet = await repository.GetAsync(id);

            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            
            var dataRow = dataSet.Tables[0].Rows[0];

            Feedback feedback = new() {
                Id = Convert.ToInt32(dataRow["Id"]),
                UserId = dataRow["UserId"].ToString() ?? "",
                Title = dataRow["Title"].ToString() ?? "",
                Description = dataRow["Description"].ToString() ?? "",
                Label = (Label)Enum.Parse(typeof(Label), dataRow["Label"].ToString() ?? "Feature")
            };

            return feedback;
        }

        public async Task CreateAsync(Feedback feedback) {
            await repository.CreateAsync(feedback);
        }

        public async Task UpdateAsync(Feedback feedback) {
            await repository.UpdateAsync(feedback);
        }

        public async Task DeleteAsync(int id) {
            await repository.DeleteAsync(id);
        }
    }
}