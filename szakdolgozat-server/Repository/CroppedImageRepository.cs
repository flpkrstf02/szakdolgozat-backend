using szakdolgozat_server.Data;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public class CroppedImageRepository : ICroppedImageRepository
    {
        DataContext db;
        static string ONNX_MODEL_PATH = "automl-model.onnx";

        public CroppedImageRepository(DataContext db)
        {
            this.db = db;
        }

        public void Add(CroppedImage croppedImage)
        {
            db.CroppedImages.Add(croppedImage);
            db.SaveChanges();
        }
        public CroppedImage GetByID(int id)
        {
            return db.CroppedImages.FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<CroppedImage> GetAll()
        {
            return db.CroppedImages;
        }
        public void Delete(int id)
        {
            var croppedImageToDelete = GetByID(id);
            db.CroppedImages.Remove(croppedImageToDelete);
            db.SaveChanges();
        }
        public void Update(CroppedImage croppedImage)
        {
            var croppedImageToUpdate = GetByID(croppedImage.Id);
            croppedImageToUpdate.Prediction = croppedImage.Prediction;
            croppedImageToUpdate.FlowerId = croppedImage.FlowerId;
            db.SaveChanges();
        }
    }
}
