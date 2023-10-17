using szakdolgozat_server.Models;
using szakdolgozat_server.Repository;

namespace szakdolgozat_server.Logic
{
    public class TrainingHistoryLogic : ITrainingHistoryLogic
    {
        ITrainingHistoryRepository trainingHistoryRepository;

        public TrainingHistoryLogic(ITrainingHistoryRepository trainingHistoryRepository)
        {
            this.trainingHistoryRepository = trainingHistoryRepository;
        }

        public void Add(TrainingHistory trainingHistory)
        {
            trainingHistoryRepository.Add(trainingHistory);
        }
        public TrainingHistory GetByID(int id)
        {
            return trainingHistoryRepository.GetByID(id);
        }
        public IEnumerable<TrainingHistory> GetAll()
        {
            return trainingHistoryRepository.GetAll();
        }
        public void Update(TrainingHistory trainingHistory)
        {
            trainingHistoryRepository.Update(trainingHistory);
        }
        public void Delete(int id)
        {
            trainingHistoryRepository.Delete(id);
        }
    }
}
