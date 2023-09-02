using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar_AspNetCore7.Data;
using RentCar_AspNetCore7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RentCar_AspNetCore7.Controllers.HomeController;

namespace RentCar_AspNetCore7.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class IndexViewModel
        {
            public DateTime Today
            {
                get; set;
            }
            public List<Car> Cars
            {
                get; set;
            }
            public List<City> Cities
            {
                get; set;
            }
            public int TotalPages
            {
                get; set;
            }
            public int CurrentPage
            {
                get; set;
            }
        }
        public class RentViewModel
        {
            public string SelectedCar
            {
                get; set;
            }
            public string RentalCity
            {
                get; set;
            }
            public string DropOffCity
            {
                get; set;
            }
            public DateTime StartDate
            {
                get; set;
            }
            public DateTime ReturnDate
            {
                get; set;
            }
            public decimal TotalPrice
            {
                get; set;
            }

            public List<Car> Cars
            {
                get; set;
            }
            public List<City> Cities
            {
                get; set;
            }
        }

        public class CarsViewModel
        {
            public List<Car> Cars
            {
                get; set;
            }
            public List<City> Cities
            {
                get; set;
            }
            public int TotalPages
            {
                get; set;
            }
            public int CurrentPage
            {
                get; set;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var randomCars = await _context.Cars.OrderBy(r => Guid.NewGuid()).Take(5).ToListAsync();
            var allCities = await _context.Cities.ToListAsync();

            var model = new IndexViewModel
            {
                Today = DateTime.Today,
                Cars = randomCars,
                Cities = allCities
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string city, DateTime journeyDate)
        {
            IQueryable<Car> cars;
            IQueryable<City> cities = _context.Cities;

            if (city == "all")
            {
                cars = _context.Cars.Where(c => c.AvailableDate <= journeyDate);
            }
            else
            {
                int cityId = int.Parse(city);
                cars = _context.Cars.Where(c => c.CityId == cityId && c.AvailableDate <= journeyDate);
            }

            IndexViewModel viewModel = new IndexViewModel
            {
                Today = DateTime.Now,
                Cars = await cars.ToListAsync(),
                Cities = await cities.ToListAsync(),
            };

            return View("CarsCityDate", viewModel);
        }




        [HttpGet]
        public IActionResult Services()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Cars(int? page)
        {
            int pageSize = 6; 
            int pageNumber = page ?? 1; 

            int totalCars = await _context.Cars.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCars / (double)pageSize); 
            var allCities = await _context.Cities.ToListAsync();

            var cars = await _context.Cars  
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            var model = new CarsViewModel
            {
                Cars = cars,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                Cities = allCities
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Rent()
        {
            await PopulateViewBagAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rent(Booking booking)
        {
            booking.TotalPrice = booking.TotalPrice / 100;
            ModelState.Remove("RentedCar");
            ModelState.Remove("DropoffCity");
            ModelState.Remove("RentalCity");
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rent successfully completed.";
                return RedirectToAction(nameof(Rent));
            }

            await PopulateViewBagAsync(); 
            return View(booking);
        }

        private async Task PopulateViewBagAsync()
        {
            ViewData["DropoffCityId"] = new SelectList(await _context.Cities.ToListAsync(), "Id", "Name");
            ViewData["RentalCityId"] = new SelectList(await _context.Cities.ToListAsync(), "Id", "Name");
            ViewData["RentedCarId"] = new SelectList(await _context.Cars.ToListAsync(), "Id", "Name");

            ViewBag.Cities = await _context.Cities.ToListAsync();
            ViewBag.Cars = await _context.Cars.ToListAsync();
        }


    }
}
