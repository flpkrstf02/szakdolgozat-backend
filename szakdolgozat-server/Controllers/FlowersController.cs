using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Newtonsoft.Json;
using szakdolgozat_server.Logic;
using szakdolgozat_server.Models;

namespace szakdolgozat_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class FlowersController : ControllerBase
    {
        private IFlowerLogic flowerLogic;
        private ICroppedImageLogic croppedImageLogic;
        private ITrainingHistoryLogic trainingHistoryLogic;
        private InferenceSession _session;

        public FlowersController(IFlowerLogic flowerLogic, ICroppedImageLogic croppedImageLogic, ITrainingHistoryLogic trainingHistoryLogic, InferenceSession session)
        {
            this.flowerLogic = flowerLogic;
            this.croppedImageLogic = croppedImageLogic;
            this.trainingHistoryLogic = trainingHistoryLogic;
            this._session = session;
        }

        [HttpGet]
        public List<FlowerDto> GetFlowers()
        {
            var flowers = flowerLogic.GetAll().ToList();
            var croppedImages = croppedImageLogic.GetAll().ToList();
            var flowersList = new List<FlowerDto>();

            foreach (var flower in flowers)
            {
                var croppedImage = croppedImages.Where(c => c.FlowerId == flower.Id).ToList();
                var flowerDto = new FlowerDto(flower, croppedImage);
                flowersList.Add(flowerDto);
            }

            return flowersList;
        }

        [HttpGet("{id}")]
        public FlowerDto GetFlower(int id)
        {
            var flower = flowerLogic.GetByID(id);
            var croppedImages = croppedImageLogic.GetAll();

            return new FlowerDto(flower, croppedImages.Where(c => c.FlowerId == flower.Id).ToList());
        }

        [HttpPut("{id}")]
        public void Put([FromBody] dynamic flower)
        {
            string rawText = flower.GetRawText();
            var input = JsonConvert.DeserializeObject<FlowerDto>(rawText);
            var flowerToUpdate = new Flower()
            {
                Id = input.Id,
                IsOverrided = true
            };
            flowerLogic.Update(flowerToUpdate);

            if (input.CroppedImage.Count() > 0)
            {
                foreach (var croppedImage in input.CroppedImage)
                {
                    var croppedImageToUpdate = new CroppedImage()
                    {
                        Id = croppedImage.Id,
                        Prediction = croppedImage.Prediction,
                        FlowerId = input.Id
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
            var prediction = flowerLogic.GetCroppedImagesFromPicture(input);

            flowerLogic.Add(new Flower() { Image = prediction.Image, IsOverrided = false });

            var lastFlower = flowerLogic.GetAll().LastOrDefault();

            foreach (var croppedImage in prediction.CroppedImages)
            {
                var modelData = new ModelData(croppedImage);

                var result = _session.Run(new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("conv2d_input", modelData.AsTensor())
                });
                var score = result.FirstOrDefault().AsTensor<float>().ToList();
                int max = 0;
                for (int i = 0; i < score.Count(); i++)
                {
                    if (score[i] > score[max])
                    {
                        max = i;
                    }
                }
                result.Dispose();

                string stage = "stage" + (max + 1);

                croppedImageLogic.Add(new CroppedImage() { Image = croppedImage, FlowerId = lastFlower.Id, Prediction = stage });
            }

        }

        [HttpDelete]
        public void Delete([FromBody] dynamic id)
        {
            string rawText = id.GetRawText();
            var input = JsonConvert.DeserializeObject<int>(rawText);
            flowerLogic.Delete(input);
        }
    }
}
