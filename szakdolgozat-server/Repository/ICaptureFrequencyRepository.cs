using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public interface ICaptureFrequencyRepository
    {
        void Add(CaptureFrequency captureFrequency);
        void Delete(int id);
        IQueryable<CaptureFrequency> GetAll();
        CaptureFrequency GetByID(int id);
        void Update(CaptureFrequency captureFrequency);
    }
}