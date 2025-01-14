using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using IOFile = System.IO.File;
namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AboutusController : Controller
    {
        private readonly DatabaseContext db;
        private readonly IWebHostEnvironment env;
        public AboutusController(DatabaseContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Aboutus.ToListAsync());
        }

        // GET: Admin/Aboutus/Create
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAdd(Aboutus aboutus)
        {
            if (ModelState.IsValid)
            {
                string imagePath = string.Empty;
                if (aboutus.ImageFile != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(aboutus.ImageFile.FileName);
                    string extension = Path.GetExtension(aboutus.ImageFile.FileName);
                    imagePath = filename + "_" + Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(env.WebRootPath, "images", imagePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await aboutus.ImageFile.CopyToAsync(stream);
                    }
                }
                var dis = new Aboutus
                {
                    AirlineName = aboutus.AirlineName,
                    Description = aboutus.Description,
                    Image = imagePath,
                    EstablishedDate = aboutus.EstablishedDate,
                };
                db.Aboutus.Add(dis);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("add", aboutus);
        }



        [HttpGet("admin/aboutus/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var about = await db.Aboutus.FindAsync(id);
            if (about == null)
            {
                return RedirectToAction("Index");
            }

            return View(about); // Confirmation page
        }
        [HttpPost("admin/aboutus/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var about = await db.Aboutus.FindAsync(id);
            if (about != null)
            {
                db.Aboutus.Remove(about);
                await db.SaveChangesAsync();
                TempData["Note"] = "About Us deleted successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}
