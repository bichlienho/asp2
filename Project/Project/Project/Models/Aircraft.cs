using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Aircraft
    {
        [Key]
        public int AircraftID { get; set; }  // Mã máy bay
        public string Model { get; set; }  // Loại máy bay (VD: Airbus A320)
        public int CabinCount { get; set; }  // Số khoang

        public ICollection<Aircraft_Ticket> Aircraft_Tickets = new List<Aircraft_Ticket>();
        public ICollection<Flight> Flights = new List<Flight>();
    }
}
