namespace szakdolgozat_server.Models
{
    public class Image_With_CroppedImage
    {
        public byte[] Image { get; set; }
        public List<byte[]> CroppedImages { get; set; }
    }
}
