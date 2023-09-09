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
            flowerRepository.Add(flower);
        }
        public Flower GetByID(int id)
        {
            return flowerRepository.GetByID(id);
        }
        public IEnumerable<Flower> GetAll()
        {
            return flowerRepository.GetAll();
        }
        public void Update(Flower flower)
        {
            flowerRepository.Update(flower);
        }
        public void Delete(int id)
        {
            flowerRepository.Delete(id);
        }

        public Image_With_CroppedImage GetCroppedImagesFromPicture(string picture)
        {
            return flowerRepository.GetCroppedImagesFromPicture(picture);
        }
    }
}
