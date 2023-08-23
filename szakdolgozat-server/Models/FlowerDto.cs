namespace szakdolgozat_server.Models
{
    public class FlowerDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool IsOverrided { get; set; }
        public List<CroppedImageDto> CroppedImage { get; set; }

        public FlowerDto(Flower flower, List<CroppedImage> croppedImages)
        {
            this.Id = flower.Flower_ID;
            this.Image = flower.Image;
            this.IsOverrided = flower.IsOverrided;
            this.CroppedImage = new List<CroppedImageDto>();

            if (croppedImages.Count > 0)
            {
                foreach (var croppedImage in croppedImages)
                {
                    this.CroppedImage.Add(new CroppedImageDto()
                    {
                        Id = croppedImage.CroppedImage_ID,
                        Image = croppedImage.Image,
                        Prediction = croppedImage.Prediction
                    });
                }
            }
        }
    }
}
