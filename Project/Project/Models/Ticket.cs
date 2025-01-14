
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Project.Models
{
	public class Ticket
	{
		[Key]
		public int TicketClassID { get; set; }
        [Required(ErrorMessage = "ClassName is required.")]
        public string ClassName { get; set; }
        [Required(ErrorMessage = "Multiplier is required.")]
        public decimal Multiplier { get; set; }
  
        public bool IsActive { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public ICollection<Flight> Flights { get; set; } = new List<Flight>();
		public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Aircraft_Ticket> Aircraft_Tickets = new List<Aircraft_Ticket>();
        public ICollection<Booking> Bookings = new List<Booking>();
    }
}
