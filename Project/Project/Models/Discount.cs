using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{

    public class Discount
    {
		[Key]
		public int DiscountID { get; set; }
        [Required]
        public string? CityDiscount { get; set; }
        [Required]
        public string? DiscountCode { get; set; }
        [Required]
        public decimal DiscountPercent { get; set; }
        [Required]
        public int QuantityDiscount { get; set; }
        [Required]
        [MaxLength(300)]
        [MinLength(10)]
        public string? Description { get; set; }

		[NotMapped]
		public IFormFile? ImageFile { get; set; }
		public string? Image { get; set; }
        [Required]
        [FutureDate]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
	}
}
