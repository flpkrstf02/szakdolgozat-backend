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
        IFlowerLogic flowerLogic;
        ICroppedImageLogic croppedImageLogic;
        private InferenceSession _session;

        public FlowersController(IFlowerLogic flowerLogic, ICroppedImageLogic croppedImageLogic, InferenceSession session)
        {
            this.flowerLogic = flowerLogic;
            this.croppedImageLogic = croppedImageLogic;
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
                //string predictedStage = croppedImageLogic.StageDetector(croppedImage);

                croppedImageLogic.Add(new CroppedImage() { Image = croppedImage, FlowerId = lastFlower.Id, Prediction = "stage1" });
            }
        }

        [HttpDelete]
        public void Delete([FromBody] dynamic id)
        {
            string rawText = id.GetRawText();
            var input = JsonConvert.DeserializeObject<int>(rawText);
            flowerLogic.Delete(input);
        }

        [HttpPost]
        [Route("/predict")]
        public void PostPrediction([FromBody] dynamic picture)
        {
            string rawText = picture.GetRawText();
            var input = JsonConvert.DeserializeObject<byte[]>(rawText);
            //string predictedStage = croppedImageLogic.StageDetector(input);

            var modelData = new ModelData(input);

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
                }0
            }

            result.Dispose();
        }

    }
}
