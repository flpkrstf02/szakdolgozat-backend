using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public interface IFlowerRepository
    {
        void Add(Flower flower);
        void Delete(int id);
        IQueryable<Flower> GetAll();
        Flower GetByID(int id);
        void Update(Flower flower);
    }
}