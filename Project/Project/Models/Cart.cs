
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
	public class Cart
	{

		[Key]
		public int Id { get; set; }
		public int? FlightId { get; set; }
        public decimal? Price { get; set; }
        [Required]
        public string? Fullname { get; set; }
        [Required]
        public string? Email { get; set; }
		public DateTime? Dob { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string? IdentityNumber { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public string? Country { get; set; }
		public int? AccId { get; set; }
        [Range(1,100, ErrorMessage = "Please choose TicketClass")]
        public int? TicketClassId { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public Account? Account { get; set; }
		public Ticket? Ticket { get; set; }
		public Flight? Flight { get; set; }


    }
}
