using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar_AspNetCore7.Data;
using RentCar_AspNetCore7.Models;

namespace RentCar_AspNetCore7.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin Admin)
        {
            // Veritabanından kullanıcıyı bul
            var adminInDb = await _context.Admins.FirstOrDefaultAsync(a => a.Username == Admin.Username);

            // Eğer kullanıcı veritabanında yoksa ya da şifre uyuşmuyorsa
            if (adminInDb == null || adminInDb.Password != Admin.Password)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(Admin);
            }

            // Oturumu başlat
            HttpContext.Session.SetString("IsAdmin", "True");

            return RedirectToAction("Index", "Admins");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

}
