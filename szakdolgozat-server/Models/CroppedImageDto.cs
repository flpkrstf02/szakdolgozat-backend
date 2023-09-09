namespace szakdolgozat_server.Models
{
    public class CroppedImageDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Prediction { get; set; }
    }
}
