using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class Aircraft_TicketController : Controller
    {
        private readonly DatabaseContext db;
        public Aircraft_TicketController(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Aircraft_Tickets.Include(a=> a.Aircraft).Include(a=> a.Ticket).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var listAircraft = await db.Aircrafts.ToListAsync();
            var listTicket = await db.Tickets.ToListAsync();

            ViewBag.Aircrafts = new SelectList(listAircraft, "AircraftID", "Model");
            ViewBag.Tickets = new SelectList(listTicket, "TicketClassID", "ClassName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Aircraft_Ticket aircraft_Ticket)
        {
            if (ModelState.IsValid)
            {
                db.Aircraft_Tickets.Add(aircraft_Ticket);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aircraft_Ticket);
        }
        public async Task<IActionResult> Edit(int id)
        {

            Aircraft_Ticket? aircraft_Ticket = await db.Aircraft_Tickets.SingleOrDefaultAsync(x => x.Id == id);
            return View(aircraft_Ticket);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Aircraft_Ticket aircraft_Ticket)
        {
            Aircraft_Ticket? oldPro = await db.Aircraft_Tickets.SingleOrDefaultAsync(p => p.Id == aircraft_Ticket.Id);
            if (oldPro != null)
            {
                if (ModelState.IsValid)
                {
                    oldPro.Quantity = aircraft_Ticket.Quantity;
                    
                    db.Aircraft_Tickets.Update(oldPro);//optional
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View();

            }
            return NotFound();//404
        }
        [HttpGet("admin/aircraft_ticket/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var aircraftticket = await db.Aircraft_Tickets.FindAsync(id);
            if (aircraftticket == null)
            {
                return RedirectToAction("Index");
            }

            return View(aircraftticket); // Confirmation page
        }
        [HttpPost("admin/aircraft_ticket/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var aircraftticket = await db.Aircraft_Tickets.FindAsync(id);
            if (aircraftticket != null)
            {
                db.Aircraft_Tickets.Remove(aircraftticket);
                await db.SaveChangesAsync();
                TempData["Note"] = "Aircraft Ticket deleted successfully!";
            }
            return RedirectToAction("Index");
        }

    }
}
