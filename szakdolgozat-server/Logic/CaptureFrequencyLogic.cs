using szakdolgozat_server.Models;
using szakdolgozat_server.Repository;

namespace szakdolgozat_server.Logic
{
    public class CaptureFrequencyLogic : ICaptureFrequencyLogic
    {
        ICaptureFrequencyRepository captureFrequencyRepository;

        public CaptureFrequencyLogic(ICaptureFrequencyRepository captureFrequencyRepository)
        {
            this.captureFrequencyRepository = captureFrequencyRepository;
        }

        public void Add(CaptureFrequency captureFrequency)
        {
            captureFrequencyRepository.Add(captureFrequency);
        }
        public CaptureFrequency GetByID(int id)
        {
            return captureFrequencyRepository.GetByID(id);
        }
        public IEnumerable<CaptureFrequency> GetAll()
        {
            return captureFrequencyRepository.GetAll();
        }
        public void Update(CaptureFrequency captureFrequency)
        {
            captureFrequencyRepository.Update(captureFrequency);
        }
        public void Delete(int id)
        {
            captureFrequencyRepository.Delete(id);
        }
    }
}
