using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace szakdolgozat_server.Models
{
    [Table("TrainingHistory")]
    public class TrainingHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int NumberOfImageAtTraining { get; set; }
        [Required]
        public string Date { get; set; }
    }
}
