using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int? FlightId { get; set; }
        public decimal? Price { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public DateTime? Dob { get; set; }
        public string? IdentityNumber { get; set; }
        public bool Gender { get; set; }
        public string? Country { get; set; }
        public int? AccId { get; set; }
        public int? TicketClassId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public Account? Account { get; set; }
        public Ticket? Ticket { get; set; }
        public Flight? Flight { get; set; }

    }
}
