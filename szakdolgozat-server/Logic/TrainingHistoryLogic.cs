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
            try
            {
                trainingHistoryRepository.Add(trainingHistory);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TrainingHistory GetByID(int id)
        {
            try
            {
                return trainingHistoryRepository.GetByID(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<TrainingHistory> GetAll()
        {
            try
            {
                return trainingHistoryRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(TrainingHistory trainingHistory)
        {
            try
            {
                trainingHistoryRepository.Update(trainingHistory);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(int id)
        {
            try
            {
                trainingHistoryRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
