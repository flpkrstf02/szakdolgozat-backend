using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public interface ITrainingHistoryRepository
    {
        void Add(TrainingHistory trainingHistory);
        void Delete(int id);
        IQueryable<TrainingHistory> GetAll();
        TrainingHistory GetByID(int id);
        void Update(TrainingHistory trainingHistory);
    }
}