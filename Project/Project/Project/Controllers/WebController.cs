using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Repository;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class WebController : Controller
    {
        private readonly IAccountRepository accountRepo;
		private readonly DatabaseContext db;
		private readonly IWebHostEnvironment env;

		public WebController(IAccountRepository accountRepo, DatabaseContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.accountRepo = accountRepo;
			this.env = webHostEnvironment;
			this.db = db;
        }
        //trang home
        [Route("/")]
        public IActionResult Index()
        {
            var discounts = db.Discounts.OrderByDescending(f => f.CreateDate)
        .Take(4).ToList();
            var pilots = db.Pilots.OrderByDescending(f => f.Name)
                .Take(4).ToList();

            var viewModel = new DiscountsAndPilotsViewModel
            {
                Discounts = discounts,
                Pilots = pilots
            };

            return View(viewModel);

        }
        //tìm flight
        [Route("/flight")]
        public IActionResult Flight(string from, string to)
        {
            if (!User.IsInRole("User"))
            {
             
                return RedirectToAction("AccessDenied","Account"); // Render access denied view
            }

            var fl = db.Flights
                .Where(f => f.IsActive && f.FlightStatusId == 2)
                .OrderByDescending(f => f.CreateDate)
                .Include(f => f.Airport) // Include FromAirport navigation property
                .Include(f => f.FlightStatus) // Include FlightStatus navigation property
                .AsQueryable();

            if (!string.IsNullOrEmpty(from))
            {
                fl = fl.Where(f => f.Airport.AirportName.Contains(from, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(to))
            {
                fl = fl.Where(f => f.Airport.AirportName.Contains(to, StringComparison.OrdinalIgnoreCase));
            }

            return View(fl.ToList());
        }

        //view all lọc chuyến bay trong 1 ngày
        [Route("/viewflight")]
        public IActionResult viewflight()
        {
            return View();
        }
        //liên hệ
        // GET: /contact

        [Route("/Contact", Name = "Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("/Contact")]
        [HttpPost]
        public async Task<IActionResult> Contact(ContactUs contact)
        {
            if (ModelState.IsValid)
            {
                db.ContactUs.Add(contact);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        //liên hệ
        [Route("/cart")]
        public async Task<IActionResult> cart()
        {
            int accountId = 1;

            //string accountId = HttpContext.Session.GetString("userId");
            var list = await db.Carts.Where(c => c.AccId == accountId).SingleOrDefaultAsync();
            return View(list);
        }

        [Route("/addToCart")]
        public async Task<IActionResult> AddToCart(int flightId, int ticketId)
        {
            int accountId = 1;
            //string accountId = HttpContext.Session.GetString("userId");
            var flight = await db.Carts.Where(c => c.AccId == accountId && c.FlightId == flightId).SingleOrDefaultAsync();
            if (flight == null)
            {
                Cart cart = new Cart
                {
                    FlightId = flightId,
                    AccId = accountId,

                    TicketClassId = ticketId,

                    CreateDate = DateTime.Now

                };
                db.Carts.Add(cart);
                await db.SaveChangesAsync();
            }
            else
            {
                db.Carts.Update(flight);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("cart");
        }

        //trang checkout
        [Route("/booking")]
        public IActionResult booking()
        {


            //var cart = Context.Session.Get<List<CartViewModel>>("Cart") ?? new List<CartViewModel>();
            return View(cart);
        }
        //trang thông tin khuyen mai
        [Route("/InfomationPromotion", Name = "InfomationPromotion")]
        public IActionResult InfomationPromotion()
        {
            var dis = db.Discounts.OrderByDescending(f => f.CreateDate)
              .Take(4).ToList();
            return View(dis);
        }
        //trang about
        [Route("/about")]
        public async Task<IActionResult> about()
        {
            var about = await db.Aboutus.ToListAsync();
            return View(about);
        }
        //trang Q&A
        [Route("/question")]
		public async Task< IActionResult> QA()
		{
			var about = await db.Feedbacks.ToListAsync();
			return View(about);
		}
		//trang Support
		[Route("/support")]
		public IActionResult support()
		{
			return View();
		}

		[Route("/EditProfile/{id}")]
		public async Task<IActionResult> EditProfile(string id)
		{
			// Kiểm tra xem ID có hợp lệ không
			if (string.IsNullOrEmpty(id))
			{
				return RedirectToAction("Error", new { message = "User ID is required." });
			}

			// Lấy thông tin người dùng từ cơ sở dữ liệu
			var user = await db.Accounts.FindAsync(id);
			if (user == null)
			{
				return RedirectToAction("Error", new { message = "User not found." });
			}

			// Gán dữ liệu người dùng vào model
			var model = new Account
			{
				Id = user.Id,
				Fullname = user.Fullname,
				Address = user.Address,
				Gender = user.Gender,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Avatar = user.Avatar,
				SkyMiles = user.SkyMiles,
				Dob = user.Dob,
				IsActive = user.IsActive
			};

			// Trả lại View với model chứa thông tin người dùng
			return View(model);
		}

		[HttpPost("/EditProfile/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditProfile(string id, Account model, IFormFile ImageFile)
		{
			// Kiểm tra xem ID có hợp lệ không
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest("ID is required");
			}

			// Lấy thông tin người dùng từ cơ sở dữ liệu
			var detail = await db.Accounts.FindAsync(id);
			if (detail == null)
			{
				Console.WriteLine($"ID not found in database: {id}");
				return NotFound("Account not found");
			}

			// Kiểm tra tính hợp lệ của model
			if (ModelState.IsValid)
			{
				try
				{
					// Xử lý ảnh đại diện
					if (ImageFile != null && ImageFile.Length > 0)
					{
						// Xóa ảnh cũ nếu có
						if (!string.IsNullOrEmpty(detail.Avatar))
						{
							var oldImagePath = Path.Combine(env.WebRootPath, "images", detail.Avatar);
							if (System.IO.File.Exists(oldImagePath))
							{
								System.IO.File.Delete(oldImagePath);
							}
						}

						// Lưu ảnh mới
						var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
						var filePath = Path.Combine(env.WebRootPath, "images", fileName);
						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await ImageFile.CopyToAsync(stream);
						}
						detail.Avatar = fileName;
					}

					// Cập nhật các thuộc tính người dùng
					detail.Fullname = model.Fullname;
					detail.Address = model.Address;
					detail.PhoneNumber = model.PhoneNumber;
					detail.Gender = model.Gender;
					detail.Dob = model.Dob;

					// Cập nhật trong cơ sở dữ liệu
					db.Update(detail);
					await db.SaveChangesAsync();

					// Chuyển hướng về trang chính sau khi cập nhật
					return RedirectToAction("Index", "Web");
				}
				catch (Exception ex)
				{
					// Xử lý lỗi nếu có
					ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
				}
			}

			// Nếu có lỗi, trả lại View với dữ liệu hiện tại
			return View(model);
		}

      
    }
}
