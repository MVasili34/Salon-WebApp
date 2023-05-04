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
        private readonly IHttpClientFactory clientFactory;

		public HomeController(ILogger<HomeController> logger,
            Salon.DataContext.SalonProjectContext db,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            this.db = db;
            this.clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Workers(string? name)
        {
            string uri;
            if (string.IsNullOrEmpty(name)) 
            {
				ViewData["Title"] = "Администрирование cотрудников";
                uri = "api/workers/";
			}
            else
            {
				ViewData["Title"] = $"Сотрудники по имени: {name}";
                uri = $"api/workers/?name={name}";
            }
            HttpClient worker = clientFactory.CreateClient("Salon.WebApi");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage responseMessage = await worker.SendAsync(request);
            IEnumerable<Worker>? model = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<Worker>>();
            return View(model);
        }

        public async Task<IActionResult> OurPricelist(string? Name)
		{
			IEnumerable<PriceList> model = await Task.FromResult(db.PriceLists.Where(p=>EF.Functions.Like(p.Service, $"%{Name}%")));
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

        public async Task<JsonResult> GetPieChartJson()
        {
           
            List<ServicePieChart> request = await (from booking in db.Bookings
              join priceList in db.PriceLists
                  on booking.ServiceId equals priceList.ServiceId
              group booking by priceList.Service into g
              select new ServicePieChart
              {
                  Service = g.Key,
                  Income = g.Sum(booking => booking.ToPay)
              }).ToListAsync();
            return Json(new { servicePieCharts=request });
           
        }

        public async Task<JsonResult> GetAreaChartJson()
        {

            List<IncomeColumnChart> request = await (from booking in db.Bookings
              where booking.BookingDate>=DateTime.Parse("21.04.2023") &&
              booking.BookingDate <= DateTime.Parse("21.05.2023") && booking.VisitMark==true
              group booking by booking.BookingDate into g
              select new IncomeColumnChart
              {
                  Data = g.Key,
                  Income = g.Sum(booking => booking.ToPay)
              }).ToListAsync();
            return Json(new { servicePieCharts = request });

        }

        public async Task<JsonResult> GetColumnChartJson()
        {
            List<WorkersIncomeChart> request =await( from b in db.Bookings
              join w in db.Workers on b.EmpleyeeId equals w.PatentId
              where b.BookingDate >= DateTime.Parse("21.04.2023") && b.BookingDate <= DateTime.Parse("21.05.2023") && b.VisitMark == true
              group new { b, w } by w.FullName into g
              select new WorkersIncomeChart { Name = g.Key, Income = g.Sum(x => x.b.ToPay) }).ToListAsync();
            return Json(new { servicePieCharts = request });
        }
    }
}