using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace szakdolgozat_server.Models
{
    [Table("Flower")]
    public class Flower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Flower_ID { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public bool IsOverrided { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<CroppedImage> CroppedImages { get; set; }
    }
}
