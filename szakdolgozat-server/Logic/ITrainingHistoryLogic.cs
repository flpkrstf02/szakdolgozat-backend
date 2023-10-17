using szakdolgozat_server.Models;

namespace szakdolgozat_server.Logic
{
    public interface ITrainingHistoryLogic
    {
        void Add(TrainingHistory trainingHistory);
        void Delete(int id);
        IEnumerable<TrainingHistory> GetAll();
        TrainingHistory GetByID(int id);
        void Update(TrainingHistory trainingHistory);
    }
}