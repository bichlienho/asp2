using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class BookingController : Controller
    {
        private readonly DatabaseContext db;
        private readonly IWebHostEnvironment env;
        public BookingController(DatabaseContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            var acc = await db.Bookings.ToListAsync();

            return View(acc);
        }
        [HttpGet("admin/booking/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return RedirectToAction("Index");
            }

            return View(booking); // Confirmation page
        }
        [HttpPost("admin/booking/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var booking = await db.Bookings.FindAsync(id);
            if (booking != null)
            {
                db.Bookings.Remove(booking);
                await db.SaveChangesAsync();
                TempData["Note"] = "Booking deleted successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}
