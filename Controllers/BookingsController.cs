using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar_AspNetCore7.Data;
using RentCar_AspNetCore7.Models;

namespace RentCar_AspNetCore7.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bookings.Include(b => b.DropoffCity).Include(b => b.RentalCity).Include(b => b.RentedCar);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.DropoffCity)
                .Include(b => b.RentalCity)
                .Include(b => b.RentedCar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["DropoffCityId"] = new SelectList(_context.Cities, "Id", "Name");
            ViewData["RentalCityId"] = new SelectList(_context.Cities, "Id", "Name");
            ViewData["RentedCarId"] = new SelectList(_context.Cars, "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Phone,RentedCarId,RentalCityId,DropoffCityId,RentalStartDate,RentalEndDate,TotalPrice,IsApproved")] Booking booking)
        {
            booking.RentedCar = await _context.Cars.FindAsync(booking.RentedCarId);
            booking.RentalCity = await _context.Cities.FindAsync(booking.RentalCityId);
            ModelState.Remove("RentedCar");
            ModelState.Remove("DropoffCity");
            ModelState.Remove("RentalCity");
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DropoffCityId"] = new SelectList(_context.Cities, "Id", "Name", booking.DropoffCityId);
            ViewData["RentalCityId"] = new SelectList(_context.Cities, "Id", "Name", booking.RentalCityId);
            ViewData["RentedCarId"] = new SelectList(_context.Cars, "Id", "Name", booking.RentedCarId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["DropoffCityId"] = new SelectList(_context.Cities, "Id", "Name", booking.DropoffCityId);
            ViewData["RentalCityId"] = new SelectList(_context.Cities, "Id", "Name", booking.RentalCityId);
            ViewData["RentedCarId"] = new SelectList(_context.Cars, "Id", "Name", booking.RentedCarId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,RentedCarId,RentalCityId,DropoffCityId,RentalStartDate,RentalEndDate,TotalPrice,IsApproved")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            booking.RentedCar = await _context.Cars.FindAsync(booking.RentedCarId);
            booking.RentalCity = await _context.Cities.FindAsync(booking.RentalCityId);
            booking.DropoffCity = await _context.Cities.FindAsync(booking.DropoffCityId);

            ModelState.Remove("RentedCar");
            ModelState.Remove("RentalCity");
            ModelState.Remove("DropoffCity");

            if (ModelState.IsValid)
            {
                if (booking.IsApproved)
                {
                    var carToUpdate = await _context.Cars.FindAsync(booking.RentedCarId);
                    if (carToUpdate != null)
                    {
                        carToUpdate.CityId = (int)booking.DropoffCityId;
                        carToUpdate.AvailableDate = booking.RentalEndDate;
                    }
                }

                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["DropoffCityId"] = new SelectList(_context.Cities, "Id", "Name", booking.DropoffCityId);
            ViewData["RentalCityId"] = new SelectList(_context.Cities, "Id", "Name", booking.RentalCityId);
            ViewData["RentedCarId"] = new SelectList(_context.Cars, "Id", "Name", booking.RentedCarId);
            return View(booking);
        }



        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.DropoffCity)
                .Include(b => b.RentalCity)
                .Include(b => b.RentedCar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
          return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
