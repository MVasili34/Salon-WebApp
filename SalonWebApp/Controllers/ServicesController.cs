using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Salon.DataContext;
using System.Data;
using System.Text;

namespace SalonWebApp.Controllers
{
	[Authorize(Roles = "Administrator")]
	public class ServicesController : Controller
	{
		private readonly ILogger<ServicesController> _logger;
		private readonly Salon.DataContext.SalonProjectContext db;
		private readonly IHttpClientFactory clientFactory;

		public ServicesController(ILogger<ServicesController> logger,
			Salon.DataContext.SalonProjectContext db,
			IHttpClientFactory clientFactory)
		{
			_logger = logger;
			this.db = db;
			this.clientFactory = clientFactory;
		}

		public IActionResult CreateService()
		{
			return View();
		}

		// POST: Services/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateService([Bind("ServiceId,Service,Price")] PriceList service2)
		{
			if (ModelState.IsValid)
			{
				HttpClient w = clientFactory.CreateClient("Salon.WebApi");
				var json = JsonConvert.SerializeObject(service2);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var response = await w.PostAsync("/api/services", content);
				return RedirectToAction("OurPricelist", "Home");
            }
			return View(service2);
        }


		public async Task<IActionResult> EditService(int id)
		{
			HttpClient service = clientFactory.CreateClient("Salon.WebApi");
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/services/{id}");
			HttpResponseMessage responseMessage = await service.SendAsync(request);
			PriceList? model = await responseMessage.Content.ReadFromJsonAsync<PriceList>();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditService([Bind("ServiceId,Service,Price")] PriceList service2)
		{
			if (ModelState.IsValid)
			{
				HttpClient w = clientFactory.CreateClient("Salon.WebApi");
				var json = JsonConvert.SerializeObject(service2);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var response = await w.PutAsync($"/api/services/{service2.ServiceId}", content);
				return RedirectToAction("OurPricelist", "Home");
            }
			return View(service2);
		}

		public async Task<IActionResult> DeleteService(int? id)
		{
			HttpClient worker = clientFactory.CreateClient("Salon.WebApi");
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/services/{id}");
			HttpResponseMessage responseMessage = await worker.SendAsync(request);
			PriceList? model = await responseMessage.Content.ReadFromJsonAsync<PriceList>();
			return View(model);
		}

		// DELETE: api/serivices/5
		[HttpPost]
		public async Task<IActionResult> DeleteConfirmed(int? id)
		{

			HttpClient w = clientFactory.CreateClient("Salon.WebApi");
			var response = await w.DeleteAsync($"/api/services/{id}");
			return RedirectToAction("OurPricelist", "Home");
		}
	}
}
