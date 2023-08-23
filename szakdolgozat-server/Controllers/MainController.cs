using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using szakdolgozat_server.Logic;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        IFlowerLogic flowerLogic;
        ICroppedImageLogic croppedImageLogic;

        public MainController(IFlowerLogic flowerLogic, ICroppedImageLogic croppedImageLogic)
        {
            this.flowerLogic = flowerLogic;
            this.croppedImageLogic = croppedImageLogic;
        }

        [HttpGet]
        public List<FlowerDto> GetFlowers()
        {
            var flowers = flowerLogic.GetAll();
            var croppedImages = croppedImageLogic.GetAll();
            var flowersList = new List<FlowerDto>();

            foreach (var flower in flowers)
            {
                flowersList.Add(new FlowerDto(flower, croppedImages.Where(c => c.Flower_ID == flower.Flower_ID).ToList()));
            }

            return flowersList;
        }

        [HttpGet("{id}")]
        public FlowerDto GetFlower(int id)
        {
            var flower = flowerLogic.GetByID(id);
            var croppedImages = croppedImageLogic.GetAll();

            return new FlowerDto(flower, croppedImages.Where(c => c.Flower_ID == flower.Flower_ID).ToList());
        }

        [HttpPut("{id}")]
        public void Put([FromBody] dynamic flower)
        {
            string rawText = flower.GetRawText();
            var input = JsonConvert.DeserializeObject<FlowerDto>(rawText);
            var flowerToUpdate = new Flower()
            {
                Flower_ID = input.Id,
                Image = input.Image,
                IsOverrided = input.IsOverrided
            };
            flowerLogic.Update(flowerToUpdate);

            if (input.CroppedImage.Count() > 0)
            {
                foreach (var croppedImage in input.CroppedImage)
                {
                    var croppedImageToUpdate = new CroppedImage()
                    {
                        CroppedImage_ID = croppedImage.Id,
                        Image = croppedImage.Image,
                        Prediction = croppedImage.Prediction,
                        Flower_ID = input.Id
                    };
                    croppedImageLogic.Update(croppedImageToUpdate);
                }
            }
        }

        [HttpPost]
        public void Post([FromBody] dynamic picture)
        {
            string rawText = picture.GetRawText();
            var input = JsonConvert.DeserializeObject<string>(rawText);


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
