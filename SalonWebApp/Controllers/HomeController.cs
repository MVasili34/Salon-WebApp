using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Salon.DataContext;
using SalonWebApp.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace SalonWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly Salon.DataContext.SalonProjectContext db;

		public HomeController(ILogger<HomeController> logger,
            Salon.DataContext.SalonProjectContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult OurPricelist()
        {
            IEnumerable<PriceList> model = db.PriceLists;
            return View(model);
        }
        [HttpPost]
		public IActionResult OurPricelist(string? Name)
		{
			IEnumerable<PriceList> model = db.PriceLists.Where(p=>EF.Functions.Like(p.Service, $"%{Name}%"));
            if (!model.Any())
            {
                OurPricelist();
            }
			return View(model);
		}

		[Authorize(Roles ="Administrator")]
        public IActionResult AdminPanel() 
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult GetPieChartJson()
        {
           
            List<ServicePieChart> request = (from booking in db.Bookings
              join priceList in db.PriceLists
                  on booking.ServiceId equals priceList.ServiceId
              group booking by priceList.Service into g
              select new ServicePieChart
              {
                  Service = g.Key,
                  Income = g.Sum(booking => booking.ToPay)
              }).ToList();
            return Json(new { servicePieCharts=request });
           
        }
    }
}