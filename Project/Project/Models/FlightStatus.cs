using Microsoft.Build.Framework;

namespace Project.Models
{
	public class FlightStatus
	{
		public int FlightStatusId { get; set; }

        public string FlightStatusName { get; set; }
        public string Description { get; set; }
        public ICollection<Flight> Flights { get; set; } = new List<Flight>();
	}
}
