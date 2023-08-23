using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace szakdolgozat_server.Models
{
    public enum Stage
    {
        FIRST, SECOND, THIRD, FOURTH
    }
    [Table("CroppedImage")]
    public class CroppedImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CroppedImage_ID { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public Stage Stage { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual Flower Flower { get; set; }

        [ForeignKey(nameof(Flower))]
        public int Flower_ID { get; set; }
    }
}