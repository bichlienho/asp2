
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
	public class Airport
	{
		public int AirportID { get; set; }
        [Required(ErrorMessage = "Airport name is required.")]
        public string? AirportName { get; set; }
        [Required(ErrorMessage = "Airport city is required.")]
        public string? City { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public bool IsActive { get; set; }

		public ICollection<Flight> Flights { get; set; } = new List<Flight>();
	}
}
