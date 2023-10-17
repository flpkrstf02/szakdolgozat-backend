using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using szakdolgozat_server.Logic;
using szakdolgozat_server.Models;
using Newtonsoft.Json;

namespace szakdolgozat_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CaptureFrequencyController : ControllerBase
    {
        ICaptureFrequencyLogic frequencyLogic;

        public CaptureFrequencyController(ICaptureFrequencyLogic frequencyLogic)
        {
            this.frequencyLogic = frequencyLogic;
        }

        [HttpGet]
        public List<CaptureFrequency> GetCaptureFrequencies()
        {
            return frequencyLogic.GetAll().ToList();
        }

        [HttpGet]
        [Route("capture")]
        public bool GetCaptureLicense()
        {
            var captureFrequencies = frequencyLogic.GetAll().ToList();
            if (captureFrequencies.Count() > 0)
            {
                int hourWhenCapture = captureFrequencies.FirstOrDefault().Hour;
                var now = DateTime.Now;
                if (hourWhenCapture == now.Hour && (now.Minute >= 0 || now.Minute <= 2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        [HttpPut("{id}")]
        public void Put([FromBody] dynamic frequency)
        {
            string rawText = frequency.GetRawText();
            var input = JsonConvert.DeserializeObject<CaptureFrequency>(rawText);

            frequencyLogic.Update(input);
        }
    }
}
