using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System.Runtime.InteropServices;

namespace Project.Controllers
{
    public class CartController : Controller
    {
        private readonly DatabaseContext db;

        public CartController(DatabaseContext db)
        {
            this.db = db;
        }
     
        public async Task<IActionResult> Index()
        {
            string username = User.Identity.Name;
            var account = await db.Accounts.FirstOrDefaultAsync(a => a.Username == username);
            var list = await db.Carts
                .Include(c => c.Ticket)
                .Include(c => c.Flight)
                .ThenInclude(c => c.Aircraft)
                .Where(c => c.AccId == account.Id)
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Add(int flightID)
        {

            Cart cart = new Cart();

            string username = User.Identity.Name;
            var account = await db.Accounts.FirstOrDefaultAsync(a => a.Username == username);


            if (account != null)
            {
                // Gán thông tin từ tài khoản
                cart.FlightId = flightID;
                cart.AccId = account.Id;
                cart.IdentityNumber = account.IdentityNumber;
                cart.Email = account.Email;
                cart.Gender = account.Gender;
                cart.Fullname = account.Fullname;
                cart.Dob = account.Dob;
                cart.AccId = account.Id;
            }

            // Lấy thông tin chuyến bay và máy bay
            cart.Flight = await db.Flights
                .Include(f => f.Aircraft)
                .ThenInclude(f => f.Aircraft_Tickets)
                .ThenInclude(f => f.Ticket)
                .SingleOrDefaultAsync(f => f.FlightID == flightID);
            List<Ticket> ticketList = new List<Ticket>();
            foreach (var item in cart.Flight.Aircraft.Aircraft_Tickets)
            {
                //tinh quantity con lai
                var bookings = await db.Bookings.Where(b => b.FlightId == flightID).ToListAsync();
                int quantityBoooking = 0;
                foreach (var booking in bookings)
                {
                    if (booking.TicketClassId == item.TicketId)
                    {
                        quantityBoooking++;
                    }
                }

                item.QuantityOnHand = item.Quantity - quantityBoooking;
                ticketList.Add(item.Ticket);

            }
            ViewBag.Tickets = new SelectList(ticketList, "TicketClassID", "ClassName");

           
            return View(cart);
        }




        [HttpPost]
     
        public async Task<IActionResult> Add(Cart cart)
        {
            // Lấy thông tin chuyến bay và máy bay
            cart.Flight = await db.Flights
                .Include(f => f.Aircraft)
                .ThenInclude(f => f.Aircraft_Tickets)
                .ThenInclude(f => f.Ticket)
                .SingleOrDefaultAsync(f => f.FlightID == cart.FlightId);
            if (ModelState.IsValid)
            {

                List<Ticket> ticketList = new List<Ticket>();
                foreach (var item in cart.Flight.Aircraft.Aircraft_Tickets)
                {
                    //tinh quantity con lai
                    var bookings = await db.Bookings.Where(b => b.FlightId == cart.FlightId).ToListAsync();
                    int quantityBoooking = 0;
                    foreach (var booking in bookings)
                    {
                        if (booking.TicketClassId == item.TicketId)
                        {
                            quantityBoooking++;
                        }
                    }
                    item.QuantityOnHand = item.Quantity - quantityBoooking;
                    ticketList.Add(item.Ticket);
                    ViewBag.Tickets = new SelectList(ticketList, "TicketClassID", "ClassName");
                }

                foreach(var item in cart.Flight.Aircraft.Aircraft_Tickets)
                {
                    if(item.TicketId == cart.TicketClassId && item.QuantityOnHand <= 0)
                    {
                        ViewBag.Error = "Not Enough Quantity!";
                        return View(cart);
                    }
                }

                var flight = await db.Flights.SingleOrDefaultAsync(f => f.FlightID == cart.FlightId);
                var ticketClass = await db.Tickets.SingleOrDefaultAsync(t => t.TicketClassID == cart.TicketClassId);
                cart.Price = flight.Price * ticketClass.Multiplier;

                db.Carts.Add(cart);
                await db.SaveChangesAsync();
                return Redirect("/Cart/Index");
            }
            return View(cart);
        }



        [HttpGet("/cart/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await db.Carts.FindAsync(id);
            if (cart == null)
            {
                return Redirect("/Cart/Index");
            }

            return View(cart); // Confirmation page
        }
        [HttpPost("/cart/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var cart = await db.Carts.FindAsync(id);
            if (cart != null)
            {
                db.Carts.Remove(cart);
                await db.SaveChangesAsync();
                TempData["Note"] = "Cart deleted successfully!";
            }
            return Redirect("/Cart/Index");
        }

        public async Task<IActionResult> AddDiscount(string code) 
        {
            string username = User.Identity.Name;
            var account = await db.Accounts.FirstOrDefaultAsync(a => a.Username == username);
            var list = await db.Carts
                .Include(c => c.Ticket)
                .Include(c => c.Flight)
                .ThenInclude(c => c.Aircraft)
                .Where(c => c.AccId == account.Id)
                .ToListAsync();
            var discount = await db.Discounts.SingleOrDefaultAsync(d=> d.DiscountCode == code            
                    && d.QuantityDiscount > 0 
                    && d.StartDate < DateTime.Now
                    && d.EndDate > DateTime.Now
                    && d.IsActive == true);
            if (discount is not null)
            {
                ViewBag.DiscountPercent = discount.DiscountPercent;
            }
            else { ViewBag.Error = "Discount code not alvalible"; }
          
            return View("Index", list);
        }


    }
}
