using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using szakdolgozat_server.Logic;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlowersController : ControllerBase
    {
        IFlowerLogic flowerLogic;
        ICroppedImageLogic croppedImageLogic;

        public FlowersController(IFlowerLogic flowerLogic, ICroppedImageLogic croppedImageLogic)
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
    }
}
