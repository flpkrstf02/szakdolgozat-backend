namespace szakdolgozat_server.Models
{
    public class FlowerDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool IsOverrided { get; set; }
        public List<CroppedImageDto> CroppedImage { get; set; }

        public FlowerDto()
        {

        }

        public FlowerDto(Flower flower, List<CroppedImage> croppedImages)
        {
            this.Id = flower.Id;
            this.Image = ConvertByteArrayToBase64String(flower.Image);
            this.IsOverrided = flower.IsOverrided;
            this.CroppedImage = new List<CroppedImageDto>();

            if (croppedImages.Count > 0)
            {
                foreach (var croppedImage in croppedImages)
                {
                    this.CroppedImage.Add(new CroppedImageDto()
                    {
                        Id = croppedImage.Id,
                        Image = ConvertByteArrayToBase64String(croppedImage.Image),
                        Prediction = croppedImage.Prediction
                    });
                }
            }
        }

        public string ConvertByteArrayToBase64String(byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray, 0, byteArray.Length, Base64FormattingOptions.None);
        }
    }
}
