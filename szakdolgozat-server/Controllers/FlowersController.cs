﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Newtonsoft.Json;
using szakdolgozat_server.Logic;
using szakdolgozat_server.Models;
using System.Diagnostics;

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

        private readonly string PATH_TO_TRAINING_FOLDER = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TRAIN_FOLDER");

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
        public void UpdateFlower([FromBody] dynamic flower)
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
        public void PostFlower([FromBody] dynamic picture)
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

            var trainingHistories = trainingHistoryLogic.GetAll().ToList();
            if (trainingHistories.Count() > 0)
            {
                var numberOfImageAtLastTraining = trainingHistories.LastOrDefault().NumberOfImageAtTraining;
                var croppedImages = croppedImageLogic.GetAll().ToList();

                if ((numberOfImageAtLastTraining + 100) < croppedImages.Count())
                {
                    string TextFileName = Path.Combine(PATH_TO_TRAINING_FOLDER, "trainingImages.txt");

                    if (System.IO.File.Exists(TextFileName))
                    {
                        System.IO.File.Delete(TextFileName);
                    }

                    System.IO.File.Create(TextFileName);

                    using (StreamWriter outputFile = new StreamWriter(TextFileName))
                    {
                        foreach (var croppedImage in croppedImages)
                        {
                            outputFile.WriteLine(croppedImage.Image);
                        }
                    }

                    string pythonScriptPath = "D:\\Szakdolgozat\\szakdolgozat-backend\\szakdolgozat-server\\train.py";

                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = pythonScriptPath;
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = true;
                    using (Process process = Process.Start(start))
                    {
                        using (StreamReader reader = process.StandardOutput)
                        {
                            string result = reader.ReadToEnd();
                            Console.Write(result);
                        }
                    }

                    trainingHistoryLogic.Add(new TrainingHistory()
                    {
                        NumberOfImageAtTraining = croppedImages.Count(),
                        Date = DateTime.Now.ToShortDateString()
                    });
                }
            }

        }

        [HttpDelete]
        public void DeleteFlower([FromBody] dynamic id)
        {
            string rawText = id.GetRawText();
            var input = JsonConvert.DeserializeObject<int>(rawText);
            flowerLogic.Delete(input);
        }
    }
}
