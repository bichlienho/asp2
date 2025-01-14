using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Picnic;
using Project.Data;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class PilotController : Controller
    {
        private readonly DatabaseContext db;
        private readonly IWebHostEnvironment env;
        public PilotController(DatabaseContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Pilots.ToListAsync());
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAdd(Pilot pilots)
        {
            if (ModelState.IsValid)
            {
                string imagePath = string.Empty;
                if (pilots.ImageFile != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(pilots.ImageFile.FileName);
                    string extension = Path.GetExtension(pilots.ImageFile.FileName);
                    imagePath = filename + "_" + Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(env.WebRootPath, "images", imagePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await pilots.ImageFile.CopyToAsync(stream);
                    }
                }
                var dis = new Pilot
                {
                    Name = pilots.Name,
                    Demo = pilots.Demo,
                    Image = imagePath,
                    /* EstablishedDate = paypols.EstablishedDate,*/
                };
                db.Pilots.Add(dis);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("add", pilots);
        }
        [HttpGet("admin/Paypol/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var paypol = await db.Pilots.FindAsync(id);
            if (paypol == null)
            {
                return RedirectToAction("Index");
            }

            return View(paypol); // Confirmation page
        }
        [HttpPost("admin/Paypol/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var paypol = await db.Pilots.FindAsync(id);
            if (paypol != null)
            {
                db.Pilots.Remove(paypol);
                await db.SaveChangesAsync();
                TempData["Note"] = "Pilot deleted successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}