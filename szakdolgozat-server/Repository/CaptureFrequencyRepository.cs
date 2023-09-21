using szakdolgozat_server.Data;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public class CaptureFrequencyRepository : ICaptureFrequencyRepository
    {
        DataContext db;

        public CaptureFrequencyRepository(DataContext db)
        {
            this.db = db;
        }

        public void Add(CaptureFrequency captureFrequency)
        {
            db.CaptureFrequencies.Add(captureFrequency);
            db.SaveChanges();
        }
        public CaptureFrequency GetByID(int id)
        {
            return db.CaptureFrequencies.FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<CaptureFrequency> GetAll()
        {
            return db.CaptureFrequencies;
        }
        public void Delete(int id)
        {
            var captureFrequencyToDelete = GetByID(id);
            db.CaptureFrequencies.Remove(captureFrequencyToDelete);
            db.SaveChanges();
        }
        public void Update(CaptureFrequency captureFrequency)
        {
            var captureFrequencyToUpdate = GetByID(captureFrequency.Id);
            captureFrequencyToUpdate.Hour = captureFrequency.Hour;
            db.SaveChanges();
        }
    }
}
