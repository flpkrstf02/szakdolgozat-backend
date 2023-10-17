using szakdolgozat_server.Data;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public class TrainingHistoryRepository : ITrainingHistoryRepository
    {
        DataContext db;

        public TrainingHistoryRepository(DataContext db)
        {
            this.db = db;
        }

        public void Add(TrainingHistory trainingHistory)
        {
            db.TrainingHistories.Add(trainingHistory);
            db.SaveChanges();
        }
        public TrainingHistory GetByID(int id)
        {
            return db.TrainingHistories.FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<TrainingHistory> GetAll()
        {
            return db.TrainingHistories;
        }
        public void Delete(int id)
        {
            var trainingHistoryToDelete = GetByID(id);
            db.TrainingHistories.Remove(trainingHistoryToDelete);
            db.SaveChanges();
        }
        public void Update(TrainingHistory trainingHistory)
        {
            var trainingHistoryToUpdate = GetByID(trainingHistory.Id);
            trainingHistoryToUpdate.NumberOfImageAtTraining = trainingHistory.NumberOfImageAtTraining;
            trainingHistoryToUpdate.Date = trainingHistory.Date;
            db.SaveChanges();
        }
    }
}
