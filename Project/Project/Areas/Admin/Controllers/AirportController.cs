using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AirportController : Controller
    {

        private readonly DatabaseContext db;
        public AirportController(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Airports.ToListAsync());
        }
        [Route("add")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAdd(Airport airport)
        {
            if (ModelState.IsValid)
            {
                db.Add(airport);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Print all errors to debug what's wrong
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage); // Log or view these in Debug output
            }

            return View("Add", airport); 
        }


        public async Task<IActionResult> Edit(int id)
        {

            Airport? airport = await db.Airports.SingleOrDefaultAsync(x => x.AirportID == id);
            return View(airport);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(  Airport airport)
        {
            Airport? oldPro = await db.Airports.SingleOrDefaultAsync(p => p.AirportID == airport.AirportID);
            if (oldPro != null)
            {
                if (ModelState.IsValid)
                {
                    oldPro.AirportName = airport.AirportName;
                    oldPro.City = airport.City;
                    oldPro.IsActive = airport.IsActive;

                    db.Airports.Update(oldPro);//optional
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View();

            }
            return NotFound();//404
        }
        [HttpGet("admin/airport/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var airport = await db.Airports.FindAsync(id);
            if (airport == null)
            {
                return RedirectToAction("Index");
            }

            return View(airport); // Confirmation page
        }
        [HttpPost("admin/airport/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var airport = await db.Airports.FindAsync(id);
            if (airport != null)
            {
                db.Airports.Remove(airport);
                await db.SaveChangesAsync();
                TempData["Note"] = "Airport deleted successfully!";
            }
            return RedirectToAction("Index");
        }

    }
}
