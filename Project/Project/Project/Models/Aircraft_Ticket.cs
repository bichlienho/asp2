using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Aircraft_Ticket
    {
        [Key]
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }

        [NotMapped]
        public int QuantityOnHand { get; set; } //so luong con lai
        public Aircraft? Aircraft { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
