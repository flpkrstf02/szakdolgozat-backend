using szakdolgozat_server.Models;

namespace szakdolgozat_server.Logic
{
    public interface ICaptureFrequencyLogic
    {
        void Add(CaptureFrequency captureFrequency);
        void Delete(int id);
        IEnumerable<CaptureFrequency> GetAll();
        CaptureFrequency GetByID(int id);
        void Update(CaptureFrequency captureFrequency);
    }
}