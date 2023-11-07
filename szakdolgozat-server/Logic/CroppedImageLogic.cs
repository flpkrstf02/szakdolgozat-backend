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
            try
            {
                croppedImageRepository.Add(croppedImage);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CroppedImage GetByID(int id)
        {
            try
            {
                return croppedImageRepository.GetByID(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CroppedImage> GetAll()
        {
            try
            {
                return croppedImageRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(CroppedImage croppedImage)
        {
            try
            {
                croppedImageRepository.Update(croppedImage);
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
                croppedImageRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
