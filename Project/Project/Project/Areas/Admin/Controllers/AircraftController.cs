using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AircraftController : Controller
    {
        private readonly DatabaseContext db;
        public AircraftController(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Aircrafts.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Aircraft aircraft)
        {
            if (ModelState.IsValid)
            {
                db.Aircrafts.Add(aircraft);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aircraft);
        }
        [HttpGet("admin/aircraft/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var aircraft = await db.Aircrafts.FindAsync(id);
            if (aircraft == null)
            {
                return RedirectToAction("Index");
            }

            return View(aircraft); // Confirmation page
        }
        [HttpPost("admin/aircraft/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var aircraft = await db.Aircrafts.FindAsync(id);
            if (aircraft != null)
            {
                db.Aircrafts.Remove(aircraft);
                await db.SaveChangesAsync();
                TempData["Note"] = "Aircraft deleted successfully!";
            }
            return RedirectToAction("Index");
        }

    }
}
