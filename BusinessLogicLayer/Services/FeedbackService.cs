using System.Data;
using System.Xml.Linq;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Services {
    public interface IFeedbackService {
        Task<DataSet> GetAllAsync();
        Task<Feedback> GetAsync(int id);
        Task CreateAsync(Feedback feedback);
        Task UpdateAsync(Feedback feedback);
        Task DeleteAsync(int id);
    }

    public class FeedbackService : IFeedbackService {
        private readonly IFeedbackRepository _repository;
        private readonly string _xmlFilePath = "C:\\Users\\liron\\School\\אתרים\\aspharmony\\AspHarmonyDatabase\\Feedbacks.xml";

        public FeedbackService(IFeedbackRepository repository) {
            _repository = repository;
        }

        public async Task<DataSet> GetAllAsync() {
            DataSet feedbacks =  await _repository.GetAllAsync();
            await BackupToXmlAsync(feedbacks);

            return feedbacks;
        }

        public async Task<Feedback> GetAsync(int id) {
            var dataSet = await _repository.GetAsync(id);

            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            
            var dataRow = dataSet.Tables[0].Rows[0];

            Feedback feedback = new() {
                Id = Convert.ToInt32(dataRow["Id"]),
                UserId = dataRow["UserId"].ToString(),
                Title = dataRow["Title"].ToString(),
                Description = dataRow["Description"].ToString(),
                Label = (Label)Enum.Parse(typeof(Label), dataRow["Label"].ToString())
            };

            return feedback;
        }

        public async Task CreateAsync(Feedback feedback) {
            await _repository.CreateAsync(feedback);

            await BackupToXmlAsync(feedback);
        }

        public async Task UpdateAsync(Feedback feedback) {
            await _repository.UpdateAsync(feedback);
            await BackupToXmlAsync(feedback);
        }

        public async Task DeleteAsync(int id) {
            await _repository.DeleteAsync(id);
            await RemoveFromXmlAsync(id);
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