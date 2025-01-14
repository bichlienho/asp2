using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class FlightController : Controller
    {
        private readonly DatabaseContext db;

        public FlightController(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {

            var flights = await db.Flights
                .Include(f => f.Airport)
                .Include(f => f.Aircraft)
                .Include(f => f.FlightStatus)
                .ToListAsync();

            return View(flights);
        }

        [Route("add")]
        public async Task<IActionResult> Add()
        {
            var listAirPort = await db.Airports.Where(a => a.IsActive).ToListAsync();
            ViewBag.Airports = new SelectList(listAirPort, "AirportID", "AirportName");

            var listAircraft = await db.Aircrafts.ToListAsync();
            ViewBag.Aircraft = new SelectList(listAircraft, "AircraftID", "Model");
            return View();
        }


        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAdd(Flight model)
        {
            var listAirPort = await db.Airports.Where(a => a.IsActive).ToListAsync();
            ViewBag.Airports = new SelectList(listAirPort, "AirportID", "AirportName");

            var listAircraft = await db.Aircrafts.ToListAsync();
            ViewBag.Aircraft = new SelectList(listAircraft, "AircraftID", "Model");

            if (model.DepartureTime <= DateTime.Now)
            {
                ModelState.AddModelError("DepartureTime", "Departure time must be a future date and time.");
            }
            if (ModelState.IsValid)
            {
                if (model.FlightStatusId == 0)
                {
                    model.FlightStatusId = 1; // Example: Scheduled
                }//mac dinh status


                //check lịch bay cua may bay
                var flights = await db.Flights.Where(f=> f.AircraftId == model.AircraftId).ToListAsync();
                foreach(var flight in flights)
                {
                    //chuyen bay moi voi aircraft phai sau khi aircraft hoan thanh chuyen + 2 tieng
                    if(model.DepartureTime < flight.ArrivalTime.AddHours(2))
                    {
                        ViewBag.error = "The plane needs to rest for 2 hours to be ready for the next flight";
                        return View("Add", model);
                    }
                }


                //check diem di va den phai khac nhai
                if(model.FromAirportID == model.ToAirportID)
                {

                    ViewBag.error = "Choose From must be different from Choose To";
                    return View("Add", model);
                }

                model.CreateDate = DateTime.Now;
                db.Flights.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Add", model); // Trả lại view Add khi có lỗi xác thực
        }

        public async Task<IActionResult> Details(int id)
        {
            // Lấy thông tin chuyến bay và máy bay
            var flight = await db.Flights
                .Include(f => f.Airport)               
                .Include(f => f.FlightStatus)
                .Include(f => f.Aircraft)
                .ThenInclude(f => f.Aircraft_Tickets)
                .ThenInclude(f => f.Ticket)
                .FirstOrDefaultAsync(f=> f.FlightID == id);
            

            foreach (var item in flight.Aircraft.Aircraft_Tickets.ToList())
            {
                //tinh quantity con lai
                var bookings = await db.Bookings.Where(b => b.FlightId == id).ToListAsync();
                int quantityBoooking = 0;
                foreach (var booking in bookings)
                {
                    if (booking.TicketClassId == item.TicketId)
                    {
                        quantityBoooking++;
                    }
                }
                item.QuantityOnHand = item.Quantity - quantityBoooking;
               // flight.Aircraft.Aircraft_Tickets.Add(item);
            }


            ViewBag.ListStatus = new SelectList(await db.FlightStatuses.ToListAsync(), "FlightStatusId", "FlightStatusName");      
            

            return View(flight);
        }

        public async Task<IActionResult> ChangeStatus(int id, int statusId)
        {
            if(statusId == 6 || statusId == 7)
            {
                //finish hoac cancel thi khong duoc doi nua
                ViewBag.error = "cannot change status when flight has finished or cancel";
            }
            var flight = await db.Flights
                .FirstOrDefaultAsync(f => f.FlightID == id);
            flight.FlightStatusId = statusId;
            db.Flights.Update(flight);
            await db.SaveChangesAsync();
            return Redirect($"/Admin/Flight/Details?id={id}");
        }
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var fli = await db.Flights.FindAsync(id);

            if (fli == null)
            {
                // Redirect to Index if the discount is not found
                return RedirectToAction("Index");
            }
       
 
            return View(fli);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Flight model)
        {
         

            if (model.DepartureTime <= DateTime.Now)
            {
                ModelState.AddModelError("DepartureTime", "Departure time must be a future date and time.");
            }
            if (ModelState.IsValid)
            {
                // Find the existing discount from the database
                var fli = await db.Flights.FindAsync(id);
                if (fli == null)
                {
                    // If discount is not found, redirect to Index
                    return RedirectToAction("Index");
                }

                // Update fields with the model values
                fli.FlightNumber = model.FlightNumber;
                fli.DepartureTime = model.DepartureTime;
                fli.ArrivalTime = model.ArrivalTime;
                fli.Price = model.Price;  // Set current timestamp
                fli.IsActive = model.IsActive;

                // Update the record in the database
                db.Flights.Update(fli);
                await db.SaveChangesAsync();

                // Redirect to Index page after successful update
                return RedirectToAction("Index");
            }

            // If ModelState is not valid, return the current model back to the Edit view
            return View(model);
        }

    }
}
