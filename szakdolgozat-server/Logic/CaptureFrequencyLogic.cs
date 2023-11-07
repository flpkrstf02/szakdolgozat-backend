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
            try
            {
                captureFrequencyRepository.Add(captureFrequency);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CaptureFrequency GetByID(int id)
        {
            try
            {
                return captureFrequencyRepository.GetByID(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CaptureFrequency> GetAll()
        {
            try
            {
                return captureFrequencyRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(CaptureFrequency captureFrequency)
        {
            try
            {
                captureFrequencyRepository.Update(captureFrequency);
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
                captureFrequencyRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
