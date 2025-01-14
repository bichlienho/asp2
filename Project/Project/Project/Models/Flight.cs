using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Flight
    {

		public int FlightID { get; set; }
        [Required]
        [StringLength(50)]
        public string? FlightNumber { get; set; }
        [Required]
        public int FromAirportID { get; set; }
        [Required]
        public int ToAirportID { get; set; }

        public int FlightStatusId { get; set; }
        [FutureDate]
        public DateTime DepartureTime { get; set; }
		public DateTime ArrivalTime { get; set; }
        [Required]
        public int Price { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public bool IsActive { get; set; }
        [ForeignKey("Aircraft")]
        public int AircraftId { get; set; }
		public Aircraft? Aircraft { get; set; }

		public Airport? Airport { get; set; }
		public FlightStatus? FlightStatus { get; set; }

		public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Booking> Bookings = new List<Booking>();
    }
}
