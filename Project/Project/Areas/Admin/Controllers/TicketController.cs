using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class TicketController : Controller
    {
        private readonly DatabaseContext db;
        public TicketController(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Tickets.ToListAsync());
        }
        [Route("add")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAdd(Ticket model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;

                db.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("add",model);
        }
        [HttpGet("admin/ticket/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return RedirectToAction("Index");
            }

            return View(ticket); // Confirmation page
        }
        [HttpPost("admin/ticket/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var ticket = await db.Tickets.FindAsync(id);
            if (ticket != null)
            {
                db.Tickets.Remove(ticket);
                await db.SaveChangesAsync();
                TempData["Note"] = "Ticket deleted successfully!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var ti = db.Tickets.FirstOrDefault(c => c.TicketClassID == id);
            if (ti == null)
            {
                return RedirectToAction("Index");
            }
            var model = new Ticket
            {
                ClassName = ti.ClassName,
                Multiplier = ti.Multiplier,
                IsActive = ti.IsActive,
            

            };
            return View(model);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ti)
        {

            if (ModelState.IsValid)
            {
                var tic = db.Tickets.FirstOrDefault(c => c.TicketClassID == id);
                if (tic == null)
                {
                    return RedirectToAction("Index");
                }
                tic.ClassName = ti.ClassName;
                tic.Multiplier = ti.Multiplier;
                tic.IsActive = ti.IsActive;
              
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(ti);
        }
    }
}
