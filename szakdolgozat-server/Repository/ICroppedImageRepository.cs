using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public interface ICroppedImageRepository
    {
        void Add(CroppedImage croppedImage);
        void Delete(int id);
        IQueryable<CroppedImage> GetAll();
        CroppedImage GetByID(int id);
        void Update(CroppedImage croppedImage);
    }
}