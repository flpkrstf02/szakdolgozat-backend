using szakdolgozat_server.Models;

namespace szakdolgozat_server.Logic
{
    public interface ICroppedImageLogic
    {
        void Add(CroppedImage croppedImage);
        void Delete(int id);
        IEnumerable<CroppedImage> GetAll();
        CroppedImage GetByID(int id);
        void Update(CroppedImage croppedImage);
    }
}