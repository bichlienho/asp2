using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Controllers
{
    public class FlightController : Controller
    {
        private readonly DatabaseContext _context;

        public FlightController(DatabaseContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Index(string searchTerm)
        {
            // Lưu giá trị tìm kiếm để hiển thị trong View
            ViewData["searchTerm"] = searchTerm;

            // Truy vấn danh sách Flight
            var flights = _context.Flights
                .Where(f => f.IsActive == true) // Lọc dữ liệu chỉ lấy bản ghi hợp lệ
                .AsQueryable();

            // Nếu có từ khóa tìm kiếm, áp dụng bộ lọc
            if (!string.IsNullOrEmpty(searchTerm))
            {
                flights = flights.Where(f => f.FlightNumber.ToLower().Contains(searchTerm.ToLower()));
            }

            // Trả danh sách chuyến bay khớp về View
            return View(flights.ToList());
        }


    }
}
