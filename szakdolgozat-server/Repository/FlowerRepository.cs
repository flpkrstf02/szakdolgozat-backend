using System.Net;
using System.Text;
using szakdolgozat_server.Data;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Repository
{
    public class FlowerRepository : IFlowerRepository
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
            flowerToUpdate.Image = flower.Image;
            flowerToUpdate.IsOverrided = flower.IsOverrided;
            db.SaveChanges();
        }

        public void GetCroppedImagesFromPicture(string picture)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(@"YOUR_IMAGE.jpg");
            string encoded = Convert.ToBase64String(imageArray);
            byte[] data = Encoding.ASCII.GetBytes(encoded);
            string api_key = "JSc3mc5d5KXzz8MJxtjT"; // Your API Key
            string DATASET_NAME = "detect-if-carnation"; // Set Dataset Name (Found in Dataset URL)

            // Construct the URL
            string uploadURL =
                    "https://api.roboflow.com/dataset/" +
                            DATASET_NAME + "/upload" +
                            "?api_key=" + api_key +
                            "&name=YOUR_IMAGE.jpg" +
                            "&split=train";

            // Service Request Config
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Configure Request
            WebRequest request = WebRequest.Create(uploadURL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            // Write Data
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            // Get Response
            string responseContent = null;
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }
        }
    }
}
