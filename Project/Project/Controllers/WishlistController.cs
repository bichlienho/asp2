using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class WishlistController : Controller
    {
        private static Wishlist wishlist = new Wishlist { ItemCount = 0 };

        [Route("/wishlist")]
        public ActionResult Index()
        {
            return View(wishlist);
        }

        [HttpPost]
        public JsonResult Create()
        {
            wishlist.ItemCount += 1;
            return Json(wishlist.ItemCount);
        }
    }
}
