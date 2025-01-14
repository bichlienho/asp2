using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using IOFile = System.IO.File;
namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class DiscountController : Controller
    {
        private readonly DatabaseContext db;
        private readonly IWebHostEnvironment env;
        public DiscountController(DatabaseContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Discounts.ToListAsync());
        }
        [Route("add")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAdd(Discount model)
        {
            if (ModelState.IsValid)
            {
                string imagePath = string.Empty;
                if (model.ImageFile != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    imagePath = filename + "_" + Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(env.WebRootPath, "images", imagePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                }
                var dis = new Discount
                {
                    CityDiscount = model.CityDiscount,
                    DiscountPercent = model.DiscountPercent,
                    QuantityDiscount = model.QuantityDiscount,
                    DiscountCode = model.DiscountCode,
                    Description = model.Description,
                    Image = imagePath,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    CreateDate = DateTime.Now,
                    IsActive = model.IsActive,
                };
                db.Discounts.Add(dis);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("add", model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Discount? discount = await db.Discounts
                .AsNoTracking() // Disable tracking for GET
                .SingleOrDefaultAsync(x => x.DiscountID == id);
            return View(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Discount discount)
        {
            Discount? oldPro = await db.Discounts.SingleOrDefaultAsync(p => p.DiscountID == discount.DiscountID);
            if (oldPro != null)
            {
                if (ModelState.IsValid)
                {
                    // Assign updated properties
                    oldPro.CityDiscount = discount.CityDiscount;
                    oldPro.DiscountPercent = discount.DiscountPercent;
                    oldPro.QuantityDiscount = discount.QuantityDiscount;
                    oldPro.Description = discount.Description;
                    oldPro.StartDate = discount.StartDate;
                    oldPro.EndDate = discount.EndDate;
                    oldPro.CreateDate = DateTime.Now;
                    oldPro.IsActive = discount.IsActive;

                    // Save changes
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(discount); // Return view if model is not valid
            }
            return NotFound(); // Return 404 if not found
        }



        [HttpGet("admin/Discount/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var discount = await db.Discounts.FindAsync(id);
            if (discount == null)
            {
                return RedirectToAction("Index");
            }

            return View(discount); // Confirmation page
        }
        [HttpPost("admin/Discount/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var discount = await db.Discounts.FindAsync(id);
            if (discount != null)
            {
                db.Discounts.Remove(discount);
                await db.SaveChangesAsync();
                TempData["Note"] = "Discount deleted successfully!";
            }
            return RedirectToAction("Index");
        }

    }
}
