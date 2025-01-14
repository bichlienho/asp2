using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using IOFile = System.IO.File;

namespace Project.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class FeedbackController : Controller
    {

        private readonly DatabaseContext db;
        private readonly IWebHostEnvironment env;
        public FeedbackController(DatabaseContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Feedbacks.ToListAsync());

        }
        [HttpGet("add")]

        public IActionResult Create()
        {
            ViewBag.Feedback = db.Feedbacks.ToList();
            return View();
        }
        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                string imagePath = string.Empty;
                if (feedback.ImageFile != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(feedback.ImageFile.FileName);
                    string extension = Path.GetExtension(feedback.ImageFile.FileName);
                    imagePath = filename + "_" + Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(env.WebRootPath, "images", imagePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await feedback.ImageFile.CopyToAsync(stream);
                    }
                }
                var dis = new Feedback
                {
                    Title = feedback.Title,
                    Image = imagePath,
                    Question = feedback.Question,
                    Description = feedback.Description,
                    Answer = feedback.Answer,
                    DescriptionAnswer = feedback.DescriptionAnswer,


                };
                db.Feedbacks.Add(dis);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Feedbacks = db.Feedbacks.ToList();
            return View("create", feedback);
        }


        [HttpGet("admin/feedback/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var feedback = await db.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return RedirectToAction("Index");
            }

            return View(feedback); // Confirmation page
        }
        [HttpPost("admin/feedback/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var feedback = await db.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                db.Feedbacks.Remove(feedback);
                await db.SaveChangesAsync();
                TempData["Note"] = "Feedback deleted successfully!";
            }
            return RedirectToAction("Index");
        }



    }
}
