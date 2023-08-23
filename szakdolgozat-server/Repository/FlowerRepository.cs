using szakdolgozat_server.Data;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public class FlowerRepository
    {
        DataContext db;

        public FlowerRepository(DataContext db)
        {
            this.db = db;
        }

        public void Add(Flower flower)
        {
            db.Flowers.Add(flower);
            db.SaveChanges();
        }
        public Flower GetByID(int id)
        {
            return db.Flowers.FirstOrDefault(x => x.Flower_ID == id);
        }
        public IQueryable<Flower> GetAll()
        {
            return db.Flowers;
        }
        public void Delete(int id)
        {
            var flowerToDelete = GetByID(id);
            db.Flowers.Remove(flowerToDelete);
            db.SaveChanges();
        }
        public void Update(Flower flower)
        {
            var flowerToUpdate = GetByID(flower.Flower_ID);
            flowerToUpdate.Image =flower.Image;
            flowerToUpdate.IsOverrided =flower.IsOverrided;
            db.SaveChanges();
        }
    }
}
