namespace szakdolgozat_server.Models
{
    public class BoundingBox
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Confidence { get; set; }
        public string Class { get; set; }
    }
}
