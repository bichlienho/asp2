using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project.Data;
using Project.EmailService;
using Project.Models;
using Project.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;
using IOFile = System.IO.File;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AccountController : Controller
    {
		private readonly DatabaseContext db;
		private readonly IWebHostEnvironment env;
		public AccountController(DatabaseContext db, IWebHostEnvironment env)
		{
			this.db = db;
			this.env = env;
		}
		public async Task<IActionResult> Index()
		{
			var acc = await db.Accounts
				.Include(r => r.Role).ToListAsync();

			return View(acc);
		}




		[HttpGet("delete/{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var acn = await db.Accounts.FindAsync(id);
			if (acn != null)
			{
				if (string.IsNullOrEmpty(acn.Avatar))
				{
					string imagePath = Path.Combine(env.WebRootPath, "images", acn.Avatar);
					if (IOFile.Exists(imagePath))
					{
						IOFile.Delete(imagePath);
					}
				}
				db.Accounts.Remove(acn);
				await db.SaveChangesAsync();
				return RedirectToAction("index");
			}
			else
			{
				return RedirectToAction("index");
			}
		}
	}
}
