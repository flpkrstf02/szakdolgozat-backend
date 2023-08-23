using szakdolgozat_server.Data;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public class CroppedImageRepository : ICroppedImageRepository
    {
        DataContext db;

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
            return db.CroppedImages.FirstOrDefault(x => x.CroppedImage_ID == id);
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
            var croppedImageToUpdate = GetByID(croppedImage.CroppedImage_ID);
            croppedImageToUpdate.Image = croppedImage.Image;
            croppedImageToUpdate.Prediction = croppedImage.Prediction;
            croppedImageToUpdate.Flower_ID = croppedImage.Flower_ID;
            db.SaveChanges();
        }
    }
}
