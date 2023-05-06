using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Salon.DataContext;
using SalonWebApp.Models;
using System;
using System.Data;
using System.Net.Http;
using System.Security.Policy;
using System.Text;

namespace SalonWebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class WorkersController : Controller
    {
        private readonly ILogger<WorkersController> _logger;
        private readonly Salon.DataContext.SalonProjectContext db;
        private readonly IHttpClientFactory clientFactory;

        public WorkersController(ILogger<WorkersController> logger,
            Salon.DataContext.SalonProjectContext db,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            this.db = db;
            this.clientFactory = clientFactory;
        }


        public async Task<IActionResult> Index(string? name)
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
        public IActionResult CreateWorker()
        {
            return View();
        }

        // POST: Workers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWorker([Bind("PatentId,FullName,TelNumber,ShiftId,PositionId")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                HttpClient w = clientFactory.CreateClient("Salon.WebApi");
                var json = JsonConvert.SerializeObject(worker);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await w.PostAsync("/api/workers", content);
                return RedirectToAction(nameof(Index));
            }
            return View(worker);
        }

        
        public async Task<IActionResult> EditWorker(string id)
        {
            HttpClient worker = clientFactory.CreateClient("Salon.WebApi");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/workers/{id}");
            HttpResponseMessage responseMessage = await worker.SendAsync(request);
            Worker? model = await responseMessage.Content.ReadFromJsonAsync<Worker>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWorker([Bind("PatentId,FullName,TelNumber,ShiftId,PositionId")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                HttpClient w = clientFactory.CreateClient("Salon.WebApi");
                var json = JsonConvert.SerializeObject(worker);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await w.PutAsync($"/api/workers/{worker.PatentId}", content);
                return RedirectToAction(nameof(Index));
            }
            return View(worker);
        }

        public async Task<IActionResult> DeleteWorker(string? id)
        {
            HttpClient worker = clientFactory.CreateClient("Salon.WebApi");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/workers/{id}");
            HttpResponseMessage responseMessage = await worker.SendAsync(request);
            Worker? model = await responseMessage.Content.ReadFromJsonAsync<Worker>();
            return View(model);
        }
       
        // DELETE: api/workers/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string? id)
        {

            HttpClient w = clientFactory.CreateClient("Salon.WebApi");
            var response = await w.DeleteAsync($"/api/workers/{id}");
            return RedirectToAction("Index");
        }
    }
}
