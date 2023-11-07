using szakdolgozat_server.Models;
using szakdolgozat_server.Repository;

namespace szakdolgozat_server.Logic
{
    public class FlowerLogic : IFlowerLogic
    {
        IFlowerRepository flowerRepository;

        public FlowerLogic(IFlowerRepository flowerRepository)
        {
            this.flowerRepository = flowerRepository;
        }

        public void Add(Flower flower)
        {
            try
            {
                flowerRepository.Add(flower);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Flower GetByID(int id)
        {
            try
            {
                return flowerRepository.GetByID(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<Flower> GetAll()
        {
            try
            {
                return flowerRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(Flower flower)
        {
            try
            {
                flowerRepository.Update(flower);
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
                flowerRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Image_With_CroppedImage GetCroppedImagesFromPicture(string picture)
        {
            try
            {
                return flowerRepository.GetCroppedImagesFromPicture(picture);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
