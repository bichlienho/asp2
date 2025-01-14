using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string? Title { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? Image { get; set; }
        [Required]
        [MaxLength(200)]
        [MinLength(2)]
        public string? Question { get; set; }
        [Required]
        [MaxLength(500)]
        [MinLength(2)]
        public string? Description { get; set; }
        [Required]
        [MaxLength(400)]
        [MinLength(2)]
        public string? Answer { get; set; }
        [Required]
        [MaxLength(400)]
        [MinLength(2)]
        public string? DescriptionAnswer { get; set; }
    }
}
