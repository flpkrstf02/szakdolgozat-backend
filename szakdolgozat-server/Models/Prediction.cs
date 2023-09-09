namespace szakdolgozat_server.Models
{
    public class Prediction
    {
        public double Time { get; set; }
        public ImageSize Image { get; set; }
        public BoundingBox[] Predictions { get; set; }
    }
}
