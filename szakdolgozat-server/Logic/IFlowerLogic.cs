using szakdolgozat_server.Models;

namespace szakdolgozat_server.Logic
{
    public interface IFlowerLogic
    {
        void Add(Flower flower);
        void Delete(int id);
        IEnumerable<Flower> GetAll();
        Flower GetByID(int id);
        void Update(Flower flower);
    }
}