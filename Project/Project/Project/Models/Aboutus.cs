using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Aboutus
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? Image { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string? AirlineName { get; set; }
        [Required]
        public string? Description { get; set; }
        // Ngày thành lập hãng hàng không
        public DateTime EstablishedDate { get; set; }
    }
}
