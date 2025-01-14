using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class FlightStatusController : Controller
    {
        private readonly DatabaseContext db;
        public FlightStatusController(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.FlightStatuses.ToListAsync());
        }
        [Route("add")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAdd(FlightStatus model)
        {
            if (ModelState.IsValid)
            {

                db.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("add",model);
        }
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fs = db.FlightStatuses.FirstOrDefault(c => c.FlightStatusId == id);
            if (fs == null)
            {
                return NotFound();
            }

            db.FlightStatuses.Remove(fs);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
