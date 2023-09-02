using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar_AspNetCore7.Data;
using RentCar_AspNetCore7.Models;
using RentCar_AspNetCore7.Utilities;

namespace RentCar_AspNetCore7.Controllers
{
    public class CarsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cars.Include(c => c.City);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Slug,UploadedPhoto,Doors,Seats,Transmission,Year,PricePerDay,IsActive,AvailableDate,CityId")] Car car)
        {
            car.City = await _context.Cities.FindAsync(car.CityId);
            car.Slug = SlugHelper.GenerateSlug(car.Name);
            ModelState.Remove("City");
            if (ModelState.IsValid)
            {
                Random rnd = new Random();
                int randomNumber = rnd.Next(1000, 9999);

                string fileExtension = Path.GetExtension(car.UploadedPhoto.FileName);

                string fileName = $"{car.Slug}_{randomNumber}{fileExtension}";

                string filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await car.UploadedPhoto.CopyToAsync(stream);
                }

                car.Photo = fileName;

                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", car.CityId);
            return View(car);
        }
        
        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", car.CityId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Slug,UploadedPhoto,Doors,Seats,Transmission,Year,PricePerDay,IsActive,AvailableDate,CityId")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            car.City = await _context.Cities.FindAsync(car.CityId);
            car.Slug = SlugHelper.GenerateSlug(car.Name);  // Varsayılan SlugHelper metodunuz

            ModelState.Remove("City");

            if (car.UploadedPhoto != null)
            {
                Random rnd = new Random();
                int randomNumber = rnd.Next(1000, 9999);

                string fileExtension = Path.GetExtension(car.UploadedPhoto.FileName);

                string fileName = $"{car.Slug}_{randomNumber}{fileExtension}";

                string filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await car.UploadedPhoto.CopyToAsync(stream);
                }

                car.Photo = fileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", car.CityId);
            return View(car);
        }


        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cars'  is null.");
            }
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
          return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
