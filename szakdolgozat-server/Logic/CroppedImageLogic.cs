using szakdolgozat_server.Models;
using szakdolgozat_server.Repository;

namespace szakdolgozat_server.Logic
{
    public class CroppedImageLogic : ICroppedImageLogic
    {
        ICroppedImageRepository croppedImageRepository;

        public CroppedImageLogic(ICroppedImageRepository croppedImageRepository)
        {
            this.croppedImageRepository = croppedImageRepository;
        }

        public void Add(CroppedImage croppedImage)
        {
            croppedImageRepository.Add(croppedImage);
        }
        public CroppedImage GetByID(int id)
        {
            return croppedImageRepository.GetByID(id);
        }
        public IEnumerable<CroppedImage> GetAll()
        {
            return croppedImageRepository.GetAll();
        }
        public void Update(CroppedImage croppedImage)
        {
            croppedImageRepository.Update(croppedImage);
        }
        public void Delete(int id)
        {
            croppedImageRepository.Delete(id);
        }
    }
}
