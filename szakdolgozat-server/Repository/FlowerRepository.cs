using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
            return db.Flowers.FirstOrDefault(x => x.Id == id);
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
            var flowerToUpdate = GetByID(flower.Id);
            flowerToUpdate.IsOverrided = flower.IsOverrided;
            db.SaveChanges();
        }

        public Image_With_CroppedImage GetCroppedImagesFromPicture(string picture)
        {
            picture = picture.Replace("data:image/jpeg;base64,", "");
            byte[] data = Convert.FromBase64String(picture);
            byte[] resizedData;
            Image image;

            using (MemoryStream ms = new MemoryStream(data))
            {
                image = Image.FromStream(ms);
            }

            var resizedImage = ResizeImage(image, 416, 416);

            using (var stream = new MemoryStream())
            {
                resizedImage.Save(stream, ImageFormat.Png);
                resizedData = stream.ToArray();
            }

            var prediction = GetPredictionsFromRoboflow(resizedData);

            Image_With_CroppedImage croppedImage = new Image_With_CroppedImage();
            croppedImage.Image = resizedData;

            foreach (var boundingBox in prediction.Predictions)
            {
                int topLeftX = Convert.ToInt32(boundingBox.X) - (Convert.ToInt32(boundingBox.Width) / 2);
                int topLeftY = Convert.ToInt32(boundingBox.Y) - (Convert.ToInt32(boundingBox.Height) / 2);
                int width = Convert.ToInt32(boundingBox.Width) + topLeftX <= 416 ? Convert.ToInt32(boundingBox.Width) : Convert.ToInt32(boundingBox.Width) - 1;
                int height = Convert.ToInt32(boundingBox.Height) + topLeftY <= 416 ? Convert.ToInt32(boundingBox.Height) : Convert.ToInt32(boundingBox.Height) - 1;

                Bitmap crop = cropImage(resizedImage, new Rectangle(topLeftX, topLeftY, width, height));

                using (var stream = new MemoryStream())
                {
                    crop.Save(stream, ImageFormat.Jpeg);
                    croppedImage.CroppedImages.Add(stream.ToArray());
                }
            }
            return croppedImage;
        }

        private Prediction GetPredictionsFromRoboflow(byte[] picture)
        {
            //byte[] imageArray = System.IO.File.ReadAllBytes(@"YOUR_IMAGE.jpg");
            string encoded = Convert.ToBase64String(picture);
            byte[] data = Encoding.ASCII.GetBytes(encoded);
            string api_key = "JSc3mc5d5KXzz8MJxtjT"; // Your API Key
            string model_endpoint = "detect-if-carnation/3"; // Set model endpoint

            // Construct the URL
            string uploadURL = "https://detect.roboflow.com/" + model_endpoint + "?api_key=" + api_key + "&name=YOUR_IMAGE.jpg";

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

            var prediction = JsonConvert.DeserializeObject<Prediction>(responseContent);

            return prediction;
        }

        private static Bitmap cropImage(Bitmap img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
